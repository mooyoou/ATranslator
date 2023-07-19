using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DebugSystem
{
    public class DebugConsole : MonoBehaviour
    {
        [Tooltip("控制台显示屏")] public TextConsoleSimulator console;
        [Tooltip("控制台滚动窗")] public ScrollRect consolescrollRect;
        [Tooltip("控制台输入器")] public TMP_InputField consoleInput;
        private DebugFSM _debugFsm;

        private void Awake()
        {
            _debugFsm = gameObject.AddComponent<DebugFSM>();

        }

        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="inputText"></param>
        public void HandleInputEnd(string inputText)
        {

            console.AddText($"> {inputText}");

            HandleSpecialCommand(inputText);

            Refresh();
        }

        /// <summary>
        /// 处理特殊指令
        /// </summary>
        /// <param name="cmd"></param>
        private void HandleSpecialCommand(string cmd)
        {
            switch (cmd.ToLower())
            {
                case "cls":
                    console.ClearScreen();
                    break;
                case "version":
                    console.AddText($" {Application.productName} : {Application.version} ");
                    break;
                case "mnitor":
                    _debugFsm.ChangeState(gameObject.AddComponent<MnitorState>());
                    break;
                case "exit":
                    _debugFsm.ChangeState(new CommonState(console));
                    break;
                default:
                    _debugFsm.ProcessInput(cmd);
                    break;
            }


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