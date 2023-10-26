using System;
using System.Collections;
using System.Collections.Generic;
using System.Explorer;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utility;

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

    }
    
    /// <summary>
    /// 顶部菜单栏-打开（新项目）
    /// </summary>
    private void OpenNewProject()
    {
        string folderPath = Misc.OpenFolderBrowserDialog(_rootExplorerNodeBtn!=null?_rootExplorerNode.FullPath:null);
        if(string.IsNullOrEmpty(folderPath)) return;
        ResetNodeBtns();
        _rootExplorerNode = new ExplorerNode(folderPath,true,0,null);
        GlobalSubscribeSys.Invoke("open_tips_window",new string[]
        {
            "Project is opening..."
        });
        StartCoroutine(InitNodesAsync(_rootExplorerNode));
        //InitNodes(_rootExplorerNode);
    }

    private void ResetNodeBtns()
    {
        if (_rootExplorerNodeBtn!=null)
        {
            Destroy(_rootExplorerNodeBtn.gameObject);
            _rootExplorerNodeBtn = null;
        }
    }

    private void InitNodes(ExplorerNode _rootExplorerNode)
    {
        if (_rootExplorerNode == null)return;
        _rootExplorerNodeBtn = Instantiate(explorerNodeP, explorerNodeRoot);
        _rootExplorerNodeBtn.Init(_rootExplorerNode,explorerNodeP);
    }
    
    private IEnumerator InitNodesAsync(ExplorerNode _rootExplorerNode)
    {
        if (_rootExplorerNode == null)yield break;
        //开启等待蒙版
        _rootExplorerNodeBtn = Instantiate(explorerNodeP, explorerNodeRoot);
        _rootExplorerNodeBtn.Init(_rootExplorerNode,explorerNodeP);
        GlobalSubscribeSys.Invoke("close_tips_window");
    }
    

}
