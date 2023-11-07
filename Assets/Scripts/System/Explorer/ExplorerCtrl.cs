using System.Collections;
using System.Collections.Generic;
using System.Explorer;
using System.Threading.Tasks;
using UI.InfiniteListScrollRect;
using UI.InfiniteListScrollRect.Runtime;
using UnityEngine;
using Utility;

public class ExplorerCtrl : MonoBehaviour
{
    [SerializeField] 
    private InfiniteListScrollRect explorerScrollRect;

    private ExplorerNodeData _rootExplorerNode;
    private ExplorerNodeData _newRootExplorerNode;
    private int _tipId;
    private string _lastChoosePath;
    private Coroutine _initNodesCoroutine;
    
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
            if (_rootExplorerNode != _newRootExplorerNode)
            {
                explorerScrollRect.ResetData(_rootExplorerNode);
            } 
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
        Task initTreeTask = Task.Run(() =>
        {
            _newRootExplorerNode = new ExplorerNodeData(rootPath,true,0,null); //建立文件结构树
        });
        yield return new WaitUntil(() => initTreeTask.IsCompleted);
        
        if (_newRootExplorerNode == null)yield break ;

        explorerScrollRect.ResetData(_newRootExplorerNode);
        _rootExplorerNode = _newRootExplorerNode;
        CloseTip();
    }
    
    /// <summary>
    /// 关闭提示窗口
    /// </summary>
    private void CloseTip()
    {
        GlobalSubscribeSys.Invoke("close_tips_window",_tipId);
    }
    
    /// <summary>
    /// 打开提示窗口
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
}
