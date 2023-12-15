using System.Collections;
using System.Collections.Generic;
using System.Config;
using System.Explorer;
using System.Threading.Tasks;
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
                ConfigSystem.InitProject(_rootExplorerNode.FullPath);//恢复旧的配置
            } 
            StopCoroutine(_initNodesCoroutine);
        });

        ConfigSystem.ConfigUpdate += ((sender, args) =>
        {
            RefreshExplorerList(args);
        });

        
        
    }

        
    /// <summary>
    /// 顶部菜单栏-打开（新项目）
    /// </summary>
    private void OpenNewProject()
    {
        //选择路径
        string lastOpenHistory = null;
        if (ConfigSystem.OpenProjectHistories.Count > 0)
        {
            lastOpenHistory = ConfigSystem.OpenProjectHistories[0];
        }
        string projectPath = Misc.OpenFolderBrowserDialog(lastOpenHistory);
        if (!string.IsNullOrEmpty(projectPath))
        {
            ConfigSystem.InitProject(projectPath);
        } //读取 或 建立 配置文件
    }

    //配置更新事件
    private void RefreshExplorerList(string projectPath)
    {
        _tipId = OpenTip(projectPath);
        ConfigSystem.AddOpenProjectHistory(projectPath);
        _initNodesCoroutine = StartCoroutine(InitNodesAsync(projectPath));
    }
    
    private  IEnumerator InitNodesAsync(string rootPath)
    {
        Task initTreeTask = Task.Run(() =>
        {
            _newRootExplorerNode = new ExplorerNodeData(rootPath,true,0,null); //建立完整文件结构树
        });
        yield return new WaitUntil(() => initTreeTask.IsCompleted);
        
        if (_newRootExplorerNode == null)yield break ;

        explorerScrollRect.ResetData(_newRootExplorerNode);
        _rootExplorerNode = _newRootExplorerNode;
        CloseTip();
        _newRootExplorerNode = null;
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
    private int OpenTip(string folderPath)
    {
        GlobalSubscribeSys.Invoke("open_tips_window",out List<object> ids,new System.Object[]
        {
            "Project is opening",
            $"{folderPath} Loading...",
            true,
            "cancel_explorer_init"
        });
        return ids[0] as int? ?? 0;
    }
}
