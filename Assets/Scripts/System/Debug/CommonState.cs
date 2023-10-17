using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugSystem
{
    public class CommonState : IState
    {
        private TextConsoleSimulator Console;
        private DebugFsm DebugFsm;
        public CommonState(TextConsoleSimulator console,DebugFsm debugFsm)
        {
            Console = console;
            DebugFsm = debugFsm;
        }
        
        public void Enter()
        {
            Console.AddText("Welcom use debug window");
        }

        public void Exit()
        {
            Console.ClearScreen();
        }

        public void HandleInput(string input)
        {
            switch (input.ToLower())
            {
                case "mnitor":
                    DebugFsm.SwitchToState(new MnitorState(Console,DebugFsm));
                    break;
            }
            
        }

        public void Update()
        {
            
        }

    }
}