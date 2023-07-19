using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DebugSystem
{
    /// <summary>
    /// Debug窗口的状态机
    /// </summary>
    public class DebugFSM :StateMachine 
    {

        private TextConsoleSimulator _console;

        public DebugFSM(TextConsoleSimulator console):base(new CommonState(console))
        {
            _console = console;
        }

        public override void ProcessInput(string input)
        {
            base.ProcessInput(input);
            
        }

        public override void ChangeState(IState newState)
        {
            
            if (newState.ToString() != CurrentState.ToString())
            {
                base.ChangeState(newState);
            }
        }


    }
}