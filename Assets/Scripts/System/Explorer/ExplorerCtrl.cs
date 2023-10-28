using System;
using System.Collections;
using System.Collections.Generic;
using System.Explorer;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utility;
using Object = System.Object;

public class ExplorerNode
{

    public String FileName;
    public String FullPath;
    public bool IsFolder;
    public int Depth;
    public List<ExplorerNode> SubExplorerNodes=new List<ExplorerNode>();
    public ExplorerNode FatherExplorerNode;
    public FileInfo FileInfo;

    public ExplorerNode(string fullPath,bool isFolder, int depth = 0,ExplorerNode fnode = null)
    {
        if (!Directory.Exists(fullPath) && !File.Exists(fullPath))
        {
            return;
        }
        
        FullPath = fullPath;
        FileName = Path.GetFileName(fullPath);
        Depth = depth;
        FatherExplorerNode = fnode;
        IsFolder = isFolder;
        if (isFolder)
        {
            UpdateSubNodes();
        }
        else
        {
            FileInfo = new FileInfo(fullPath);
        }
    }

    public void UpdateSubNodes()
    {
        if(!IsFolder)return;
        string[] fileEntries = Directory.GetFiles(FullPath);
        string[] subdirectoryEntries = Directory.GetDirectories(FullPath);
        foreach (string subdirectory in subdirectoryEntries)
        {
            SubExplorerNodes.Add(new ExplorerNode(subdirectory,true,Depth+1,this));
        }
        foreach (string fileEntry in fileEntries)
        {
            SubExplorerNodes.Add(new ExplorerNode(fileEntry,false,Depth+1,this));
        }
    }
}

public class ExplorerCtrl : MonoBehaviour
{
    [SerializeField] 
    private Transform explorerNodeRoot;
    [SerializeField] 
    private ExplorerNodeBtn explorerNodeP;
    
    private ExplorerNodeBtn _rootExplorerNodeBtn;
    private ExplorerNode _rootExplorerNode;
    private int _tipId;
        
    private Coroutine _initNodesCoroutine;
    private void Awake()
    {
        RegisterEvent();
    }

    public void RegisterEvent()
    {
        GlobalSubscribeSys.Subscribe("open_new_project", (objects) =>
        {
            OpenNewProject();
        });
        GlobalSubscribeSys.Subscribe("cancel_explorer_init", (objects) =>
        {
            StopCoroutine(_initNodesCoroutine);
            ResetNodeBtns();
        });
    }
    
    /// <summary>
    /// 顶部菜单栏-打开（新项目）
    /// </summary>
    private void OpenNewProject()
    {
        string folderPath = Misc.OpenFolderBrowserDialog(_rootExplorerNodeBtn!=null?_rootExplorerNode.FullPath:null);
        if(string.IsNullOrEmpty(folderPath)) return;
        
        GlobalSubscribeSys.Invoke("open_tips_window",out List<object> ids,new Object[]
        {
            "Project is opening",
            $"{folderPath} Loading...",
            true,
            "cancel_explorer_init"
        });
        _tipId = ids[0] as int? ?? 0;

        _initNodesCoroutine = StartCoroutine(InitNodesAsync(folderPath));
    }
    
    private void ResetNodeBtns()
    {
        if (_rootExplorerNodeBtn!=null)
        {
            Destroy(_rootExplorerNodeBtn.gameObject);
            _rootExplorerNodeBtn = null;
        }
    }

    private IEnumerator InitNodesAsync(string rootPath)
    {
        ResetNodeBtns();
        _rootExplorerNode = new ExplorerNode(rootPath,true,0,null);
        if (_rootExplorerNode == null)yield break;
        _rootExplorerNodeBtn = Instantiate(explorerNodeP, explorerNodeRoot);
        _rootExplorerNodeBtn.Init(_rootExplorerNode,explorerNodeP);
         yield return new WaitForSeconds(1);
        GlobalSubscribeSys.Invoke("close_tips_window",_tipId);
    }
    

}
