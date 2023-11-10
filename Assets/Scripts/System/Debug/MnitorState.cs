using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DebugSystem
{

    public class MnitorState :IState
    {
        private TextConsoleSimulator Console;
        private DebugFsm DebugFsm;
        private string TipText = ""; 
        
        
        private enum MnitorEnum
        {
            eventlist,
            Null
        };

        private MnitorEnum curState = MnitorEnum.Null;
        
        public MnitorState(TextConsoleSimulator console,DebugFsm debugFsm)
        {
            Console = console;
            DebugFsm = debugFsm;
        }
        
        public void Enter()
        {
            Console.CloseTextPlayAni();
            Console.AddText("调试模式已开启");
        }

        public void Exit()
        {
            Console.OpenTextPlayAni();
            Console.AddText("调试模式已退出");
        }
        
        public void HandleInput(string input)
        {
            if (MnitorEnum.TryParse(input.ToLower(),out curState))
            {
                TipText = $"——Current Mnitor : {curState.ToString()}";
            }
            else
            {
                curState = MnitorEnum.Null;
            }
        }
        
        public void Update()
        {
            if (curState != MnitorEnum.Null)
            {
                Console.ClearScreen();
                Console.AddText(TipText);
                switch (curState)
                {
                    case MnitorEnum.eventlist:
                        EventListPrint();
                        break;
                }

                
            }
        }

        private void EventListPrint()
        {
            Console.AddText("——Current Null Return Events ——");
            foreach (var kEvent in GlobalSubscribeSys.Events)
            {
                Console.AddText($"{kEvent.Key}:{kEvent.Value.Count}");
            }
            Console.AddText("——Current Return Events ——");
            foreach (var kEvent in GlobalSubscribeSys.ReturnEvents)
            {
                Console.AddText($"{kEvent.Key}:{kEvent.Value.Count}");
            }
            
        }


    }
}