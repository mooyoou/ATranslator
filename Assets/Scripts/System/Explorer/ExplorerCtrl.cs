using System;
using System.Collections;
using System.Collections.Generic;
using System.Explorer;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
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
            Task.Run(() =>
            {
                SubExplorerNodes.Add(new ExplorerNode(subdirectory,true,Depth+1,this));
            });

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
    [SerializeField] 
    private ScrollRect scrollRect;
    
    private ExplorerNodeBtn _rootExplorerNodeBtn;
    private ExplorerNodeBtn _oldRootExplorerNodeBtn;
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
            if (_oldRootExplorerNodeBtn != null && _oldRootExplorerNodeBtn != _rootExplorerNodeBtn)
            {
                Destroy(_rootExplorerNodeBtn.gameObject);
                _rootExplorerNodeBtn = _oldRootExplorerNodeBtn;
            }

        });
    }
    
    /// <summary>
    /// 顶部菜单栏-打开（新项目）
    /// </summary>
    private void OpenNewProject()
    {
        string folderPath = Misc.OpenFolderBrowserDialog(_rootExplorerNodeBtn!=null?_rootExplorerNode.FullPath:null);
        if(string.IsNullOrEmpty(folderPath)) return;
        
        GlobalSubscribeSys.Invoke("open_tips_window",out List<object> ids,new System.Object[]
        {
            "Project is opening",
            $"{folderPath} Loading...",
            true,
            "cancel_explorer_init"
        });
        _tipId = ids[0] as int? ?? 0;

        _initNodesCoroutine = StartCoroutine(InitNodesAsync(folderPath));
    }

    private IEnumerator InitNodesAsync(string rootPath)
    {
        //保底加载时间用于用户取消操作
        yield return new WaitForSeconds(1);
        _oldRootExplorerNodeBtn = _rootExplorerNodeBtn;
        _rootExplorerNode = new ExplorerNode(rootPath,true,0,null);
        if (_rootExplorerNode == null)yield break;
        _rootExplorerNodeBtn = Instantiate(explorerNodeP, explorerNodeRoot);
        _rootExplorerNodeBtn.Init(_rootExplorerNode,explorerNodeP);
        GlobalSubscribeSys.Invoke("close_tips_window",_tipId);
        if (_oldRootExplorerNodeBtn != null)
        {
            Destroy(_oldRootExplorerNodeBtn.gameObject);
        }
        _oldRootExplorerNodeBtn = null;
    }

    public void OnVerticalChange(Single value)
    {
        // var scrollRectTransform = scrollRect.transform;
        // var position = scrollRectTransform.position;
        // var lossyScale = scrollRectTransform.lossyScale;
        // float scrollRectPosYMin = position.y - scrollRect.viewport.rect.height*lossyScale.y;
        // float scrollRectPosYMax = position.y;
        //
        // Stack<ExplorerNodeBtn> checkBtnStack = new Stack<ExplorerNodeBtn>();
        // checkBtnStack.Push(_rootExplorerNodeBtn);
        // while (checkBtnStack.Count>0)
        // {
        //     ExplorerNodeBtn curNode = checkBtnStack.Pop();
        //     // Debug.Log($"{curNode.ExplorerNode.FileName} {curNode.gameObject.transform.position}");
        //     var curNodePosition = curNode.transform.position;
        //     bool isVisible = (curNodePosition.y >= scrollRectPosYMin && curNodePosition.y <= scrollRectPosYMax);
        //     if (isVisible)
        //     {
        //         
        //         curNode.gameObject.SetActive(true);
        //     }
        //     else
        //     {
        //         curNode.gameObject.SetActive(false);
        //     }
        //
        //     foreach (var explorerNodeBtn in curNode.GetSubExplorerNodeBtns())
        //     {
        //         checkBtnStack.Push(explorerNodeBtn);
        //     }
        // }
    }

}
