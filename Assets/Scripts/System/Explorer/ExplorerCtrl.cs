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
    [SerializeField] 
    private VerticalLayoutGroup verticalLayoutGroup;
    
    //列表对象池最大数量
    private const int ScrollPoolMaxNum=40;
    //当前显示节点数
    private int _curShowNodeNum = 0;
    //当前最大节点数
    private int _curMaxNodeNum = 0;
    //当前显示节点的前置节点数量
    private int _curSkipNodeNum = 0;
    
    
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
        });

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
        // 自动计算适合对象池数量
        // var prefabHight = ((RectTransform)explorerNodeP.transform).sizeDelta.y;
        // ScrollPoolMaxNum = (int)((explorerScrollRect.viewRect.rect.size.y - verticalLayoutGroup.padding.bottom -verticalLayoutGroup.padding.top) / (prefabHight+verticalLayoutGroup.spacing))+5;
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
        if (InitBtnList.Count == 0)
        {
            var explorerNodeBtn = Instantiate(explorerNodeP, explorerNodeRoot);
            explorerNodeBtn.Init(rootNode,this);
            InitBtnList.Add(explorerNodeBtn);
        }
        else
        {
            InitBtnList[0].Init(rootNode,this);
            InitBtnList[0].gameObject.SetActive(true);
            for (int i = 1; i < InitBtnList.Count; i++)
            {
                InitBtnList[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateBtns(int skipNum=0)
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
            
            if (skipNum>0)
            {
                skipNum--;
                continue;
            }
            
            
            if (newNodeNum < InitBtnList.Count)
            {
                InitBtnList[newNodeNum].gameObject.SetActive(true);
                InitBtnList[newNodeNum].Init(curNode,this);
                
            }
            else {
                if (InitBtnList.Count > ScrollPoolMaxNum)
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
        
        //显示范围大小计算
        explorerScrollRect.customVerticalSize = (float)newNodeNum / allNodeNum;
        _curShowNodeNum = newNodeNum;
        _curMaxNodeNum = allNodeNum;
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


    /// <summary>
    /// 无限列表纵向拖动事件
    /// </summary>
    /// <param name="value">1 to 0</param>
    public void OnVerticalChange(Single value)
    {
        
        //前面未显示数量
        int needSkipNumber =(int)((_curMaxNodeNum-_curShowNodeNum)*(1-value));
        if (_curMaxNodeNum - needSkipNumber < _curShowNodeNum) needSkipNumber = _curMaxNodeNum - _curShowNodeNum;
        if (_curSkipNodeNum != needSkipNumber)
        {
            UpdateBtns(needSkipNumber);
            _curSkipNodeNum = needSkipNumber;
        }

    }

}
