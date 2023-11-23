using System.Config;
using System.Explorer;
using System.Windows.Forms;
using System.WorkSpace;
using UnityEngine;

namespace System
{
    public class MainCtl : MonoBehaviour
    {
        [SerializeField]
        private WorkspaceWindow workspaceWindow;


        private void Awake()
        {
            ApplicationInit();
            RegisterEvent();
        }

        private void ApplicationInit()
        {
            ConfigSystem.RefreshGlobbalPlayerPrefs();
        }
    
        private void RegisterEvent()
        {
            GlobalSubscribeSys.Subscribe("choose_open_file",(objects =>
            {
                if (objects.Length > 0)
                {

                    ExplorerNodeData exploreNode = objects[0] as ExplorerNodeData;

                    OpenWorkSpaceWindow(exploreNode);
                }
            } ));
        }
        
        private void OpenWorkSpaceWindow(ExplorerNodeData explorerNodeData)
        {
            if(!workspaceWindow.gameObject.activeSelf){
                workspaceWindow.gameObject.SetActive(true);
            }
            workspaceWindow.CreateTapAndOpenFile(explorerNodeData);
        }

    }
}
