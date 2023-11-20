using System.Explorer;
using UnityEngine;

namespace System.WorkSpace
{
    public class WorkspaceWindow : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorerNodeData">文件信息</param>
        internal void OpenFile(ExplorerNodeData explorerNodeData)
        {
            Debug.Log(explorerNodeData.FullPath);
            
        }
    }
}
