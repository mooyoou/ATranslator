using System.Collections;
using System.Collections.Generic;
using System.Explorer;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace System.WorkSpace
{
    public class WorkspaceWindow : MonoBehaviour
    {
        [SerializeField] private Transform fileTabRoot;
        [SerializeField] private FileTab fileTabP;
        [SerializeField] private CannotDragScrollRect fileTabScrollRect;
        [SerializeField] private TextWindowCtrl textWindowCtrl;
         
        private Dictionary<ExplorerNodeData,FileTab> openFiles = new Dictionary<ExplorerNodeData, FileTab>();
        private ExplorerNodeData curOpenFile;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="explorerNodeData">文件信息</param>
        internal void CreateTapAndOpenFile(ExplorerNodeData explorerNodeData)
        {
            if (openFiles.ContainsKey(explorerNodeData))
            {
                if (curOpenFile != explorerNodeData)
                {
                    if (openFiles.TryGetValue(explorerNodeData, out FileTab chooseFileTab))
                    {
                        chooseFileTab.OnTabClick();
                    }
                }
                return;
            }
            
            FileTab fileTap = Instantiate(fileTabP, fileTabRoot);
            fileTap.Init(explorerNodeData,ChooseFileTapAction,CloseFileTapAction);
            openFiles.Add(explorerNodeData,fileTap);
            fileTap.OnTabClick();
        }
        
        private void ChooseFileTapAction(ExplorerNodeData chooseExplorerNodeData,RectTransform chooseFileTapRectTransform)
        {
            StartCoroutine(TapLateScrollToTarget(chooseFileTapRectTransform));
            if(chooseExplorerNodeData == curOpenFile)return;//重复点击页签
            if (curOpenFile != null)
            {
                if (openFiles.TryGetValue(curOpenFile, out FileTab oldFileTab))
                {
                    oldFileTab.CancleChooseState();
                }
            }
            curOpenFile = chooseExplorerNodeData;

            if(!textWindowCtrl.gameObject.activeSelf)textWindowCtrl.gameObject.SetActive(true);
            textWindowCtrl.OpenFile(curOpenFile);
        }

        public IEnumerator TapLateScrollToTarget(RectTransform rectTransform)
        {
            yield return new WaitForEndOfFrame();
            fileTabScrollRect.ScrollToTarget(rectTransform);
        }
        
        private void CloseFileTapAction(ExplorerNodeData chooseExplorerNodeData,RectTransform chooseFileTapRectTransform)
        {
            if (openFiles.TryGetValue(chooseExplorerNodeData, out FileTab oldFileTab))
            {
                openFiles.Remove(chooseExplorerNodeData);
                if (chooseExplorerNodeData == curOpenFile)
                {
                    //关闭正在打开状态的页签的处理
                    if(openFiles.Count!=0)
                    {
                        var newFile = openFiles.Keys.ToArray()[0];
                        if (openFiles.TryGetValue(newFile, out FileTab newFileTab))
                        {
                            newFileTab.OnTabClick();
                        }
                    }
                    else
                    {
                        textWindowCtrl.gameObject.SetActive(false);
                        curOpenFile = null;
                    }
                }
                Destroy(oldFileTab.gameObject);
            }
        }
    }
}
