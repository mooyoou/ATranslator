using System;
using System.Collections;
using System.Collections.Generic;
using System.Explorer;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

    public bool IsExpand;
    
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
    private ExplorerNodeBtn_fix explorerNodeP;
    [SerializeField] 
    private ExplorerScrollRect explorerScrollRect;
    
    
    private ExplorerNode _rootExplorerNode;
    private int _tipId;
    private string _lastChoosePath;
    private Coroutine _initNodesCoroutine;

    private List<ExplorerNodeBtn_fix> InitBtnList = new List<ExplorerNodeBtn_fix>();
    private void Awake()
    {
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        GlobalSubscribeSys.Subscribe("open_new_project", (objects) =>
        {
            OpenNewProject();
        });
        GlobalSubscribeSys.Subscribe("cancel_explorer_init", (objects) =>
        {
            StopCoroutine(_initNodesCoroutine);
            ClearOldNodeBtn();
        });

    }
    
    
    /// <summary>
    /// 清除旧列表
    /// </summary>
    private void ClearOldNodeBtn()
    {

    }
    /// <summary>
    /// 顶部菜单栏-打开（新项目）
    /// </summary>
    private void OpenNewProject()
    {
        //选择路径
        _lastChoosePath = Misc.OpenFolderBrowserDialog(_lastChoosePath);
        if(string.IsNullOrEmpty(_lastChoosePath)) return;

        OpenTip(_lastChoosePath);
        _initNodesCoroutine = StartCoroutine(InitNodesAsync(_lastChoosePath));

    }

    private  IEnumerator InitNodesAsync(string rootPath)
    {
        Task InitTreeTask = Task.Run(() =>
        {
            _rootExplorerNode = new ExplorerNode(rootPath,true,0,null); //建立文件结构树
        });
        yield return new WaitUntil(() => InitTreeTask.IsCompleted);
        
        if (_rootExplorerNode == null)yield break ;
        InitBtns(_rootExplorerNode);
        CloseTip();
    }


    private void InitBtns(ExplorerNode rootNode)
    {
        explorerScrollRect.verticalScrollbar.value=0;
        var explorerNodeBtn = Instantiate(explorerNodeP, explorerNodeRoot);
        explorerNodeBtn.Init(rootNode,this);
        InitBtnList.Add(explorerNodeBtn);
    }

    private void UpdateBtns()
    {
        Stack<ExplorerNode> nodeStack = new Stack<ExplorerNode>();
        nodeStack.Push(_rootExplorerNode);
        int newNodeNum = 0;
        int allNodeNum = 0;
        while (nodeStack.Count > 0)
        {
            ExplorerNode curNode = nodeStack.Pop();
            allNodeNum++;
            
            if (curNode.IsFolder && curNode.IsExpand)
            {
                foreach (var nodeBtn in curNode.SubExplorerNodes)
                {
                    nodeStack.Push(nodeBtn);
                }
            }
            
            if (false)
            {
                //计算跳过的节点
            }
            
            
            if (newNodeNum < InitBtnList.Count)
            {
                InitBtnList[newNodeNum].gameObject.SetActive(true);
                InitBtnList[newNodeNum].Init(curNode,this);
                
            }
            else {
                if (InitBtnList.Count > 50)
                {
                    continue;
                }
                //还可以生成新节点
                var explorerNodeBtn = Instantiate(explorerNodeP, explorerNodeRoot);
                explorerNodeBtn.Init(curNode,this);
                InitBtnList.Add(explorerNodeBtn);
            }
            newNodeNum++;
        }

        if (newNodeNum < InitBtnList.Count)
        {
            for (int i = newNodeNum; i < InitBtnList.Count; i++)
            {
                InitBtnList[i].gameObject.SetActive(false);
            }
        }

        explorerScrollRect.customVerticalSize = (float)newNodeNum / allNodeNum;

    }
    
    public void ExpendBtn(ExplorerNode explorerNode)
    {
        float barValue = explorerScrollRect.verticalScrollbar.value;
        float barSize = explorerScrollRect.verticalScrollbar.size;
        UpdateBtns();
    }
    
    /// <summary>
    /// 关闭提示窗口
    /// </summary>
    private void CloseTip()
    {
        GlobalSubscribeSys.Invoke("close_tips_window",_tipId);
        // if (_oldRootExplorerNodeBtn != null)
        // {
        //     Destroy(_oldRootExplorerNodeBtn.gameObject);
        // }
        // _oldRootExplorerNodeBtn = null;
    }
    
    /// <summary>
    /// 关闭提示窗口
    /// </summary>
    /// <param name="folderPath"></param>
    private void OpenTip(string folderPath)
    {
        GlobalSubscribeSys.Invoke("open_tips_window",out List<object> ids,new System.Object[]
        {
            "Project is opening",
            $"{folderPath} Loading...",
            true,
            "cancel_explorer_init"
        });
        _tipId = ids[0] as int? ?? 0;
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
