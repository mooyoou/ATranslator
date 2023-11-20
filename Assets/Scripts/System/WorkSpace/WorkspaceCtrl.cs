using System.Explorer;
using UnityEngine;

namespace System.WorkSpace
{
    public class WorkspaceCtrl : MonoBehaviour
    {
        [SerializeField]
        private WorkspaceWindow workspaceWindow;


        private void Awake()
        {
            RegisterEvent();
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
            workspaceWindow.OpenFile(explorerNodeData);
        }
    }
}
