using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugSystem
{
    public class MnitorState : MonoBehaviour,IState
    {
        private TextConsoleSimulator _textConsoleSimulator;
        private Coroutine mnitorCoroutine;
        
        public MnitorState(TextConsoleSimulator textConsoleSimulator)
        {
            _textConsoleSimulator = textConsoleSimulator;
        }
        public void Enter()
        {
            _textConsoleSimulator.ClearScreen();
            mnitorCoroutine = StartCoroutine(StartMnitorCoroutine());
            
        }

        public void Exit()
        {
            StopCoroutine(mnitorCoroutine);
        }

        public void HandleInput(string input)
        {
            switch (input)
            {
                default:
                    _textConsoleSimulator.AddText("Invalid user command!");
                    break;
            }

        }

        private IEnumerator StartMnitorCoroutine()
        {
            while (true)
            {
                _textConsoleSimulator.AddText(" Mnitor mode running!");
                yield return new WaitForSeconds(1f); // 再等待3秒钟
            }
        }


    }
}