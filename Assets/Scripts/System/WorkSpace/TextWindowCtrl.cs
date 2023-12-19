using System.Collections;
using System.Collections.Generic;
using System.Config;
using System.Explorer;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace System.WorkSpace
{
    //TODO 多个窗口集成到一个ui文档内
    public class TextWindowCtrl : MonoBehaviour
    {
        /// <summary>
        /// 文本窗口预制体
        /// </summary>
        [SerializeField] private TextWinow textWinowPrefab;
        /// <summary>
        /// 用于文本窗口预制体实例化的根节点
        /// </summary>
        [SerializeField] private Transform textAreaRoot;

        /// <summary>
        /// 当前已加载文本窗口
        /// </summary>
        private Dictionary<ExplorerNodeData, TextWinow> openFileWindows = new Dictionary<ExplorerNodeData, TextWinow>();

        /// <summary>
        /// 当前打开文件
        /// </summary>
        private ExplorerNodeData curOpenExplorerNode;
        
        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            TextWindowEvent.TextChange += OnTextLineDatasUpdate;
        }
        
        private void UnRegisterEvents()
        {
            TextWindowEvent.TextChange = null;
        }
        
        /// <summary>
        /// 关闭（销毁）指定窗口
        /// </summary>
        /// <param name="explorerNodeData"></param>
        public void CloseWin(ExplorerNodeData explorerNodeData)
        {
            if (openFileWindows.TryGetValue(explorerNodeData, out TextWinow textWinow))
            {
                textWinow.CheckAndSaveFile();
                Destroy(textWinow.gameObject);
                openFileWindows.Remove(explorerNodeData);
            }
        }

        //文本数据发生变化
        private void OnTextLineDatasUpdate(TextLineData textLineData)
        {
            //Debug.Log(textLineData);
        }
        
        
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="explorerNodeData"></param>
        public void OpenFile(ExplorerNodeData explorerNodeData)
        {
            if (openFileWindows.TryGetValue(explorerNodeData, out TextWinow newTextWinow))
            {
                newTextWinow.SetDisplay(true);
                // newTextWinow.gameObject.SetActive(true);
            }
            else
            {
                newTextWinow = Instantiate(textWinowPrefab,textAreaRoot);
                if (!newTextWinow.InitWindow(explorerNodeData))
                {
                    Destroy(newTextWinow);
                    Debug.LogError("文件打开错误！");//TODO Tip提示窗口;
                    return;
                }
                openFileWindows.Add(explorerNodeData,newTextWinow);
                newTextWinow.SetDisplay(true);
            }
            if (curOpenExplorerNode!= null && openFileWindows.TryGetValue(curOpenExplorerNode, out TextWinow oldTextWinow))
            {
                oldTextWinow.SetDisplay(false);
            }

            curOpenExplorerNode = explorerNodeData;
        }
    }
}
