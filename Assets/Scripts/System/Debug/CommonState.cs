using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugSystem
{
    public class CommonState : IState
    {
        private TextConsoleSimulator _textConsoleSimulator;
        public CommonState(TextConsoleSimulator textConsoleSimulator)
        {
            _textConsoleSimulator = textConsoleSimulator;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void HandleInput(string input)
        {

        }
    }
}