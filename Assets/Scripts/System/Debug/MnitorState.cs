using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugSystem
{
    public class MnitorState :IState
    {
        private TextConsoleSimulator Console;
        private DebugFsm DebugFsm;
        public MnitorState(TextConsoleSimulator console,DebugFsm debugFsm)
        {
            Console = console;
            DebugFsm = debugFsm;
        }
        
        public void Enter()
        {
            Console.AddText("调试模式已开启");
        }

        public void Exit()
        {
            Console.AddText("调试模式已退出");
        }
        
        public void HandleInput(string input)
        {

        }

        
        public void Update()
        {
        }
        

    }
}