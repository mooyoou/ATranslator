using System;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DebugSystem
{
    public class DebugConsole : MonoBehaviour
    {
        [Tooltip("控制台显示屏")] public TextConsoleSimulator console;
        [Tooltip("控制台滚动窗")] public ScrollRect consolescrollRect;
        [Tooltip("控制台输入器")] public TMP_InputField consoleInput;
        private DebugFsm _debugFsm;

        private void Awake()
        {
            _debugFsm = new DebugFsm(console);
        }


        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="inputText"></param>
        public void HandleInputEnd(string inputText)
        {

            console.AddText($"> {inputText}");

            _debugFsm.ProcessInput(inputText);

            Refresh();
        }

        private void Update()
        {
            _debugFsm.UpdateState();
        }

        
        /// <summary>
        /// 屏幕刷新
        /// </summary>
        private void Refresh()
        {
            Canvas.ForceUpdateCanvases();
            consolescrollRect.verticalNormalizedPosition = 0f;
            consoleInput.text = "";
        }
    }
}