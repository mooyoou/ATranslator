using UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Update = Unity.VisualScripting.Update;

namespace DebugSystem
{

    /// <summary>
    /// Debug窗口的状态机
    /// </summary>
    public class DebugFsm :StateMachine 
    {

        private TextConsoleSimulator _console;

        public DebugFsm(TextConsoleSimulator console)
        {
            CurrentState = new CommonState(console, this);
            _console = console;
        }

        public string GetCurrentState()
        {
            return CurrentState.ToString();
        }
        
        public override void ProcessInput(string input)
        {
            ///一些各状态的公共方法写这了
            switch (input.ToLower())
            {
                case "cls":
                    _console.ClearScreen();
                    break;
                case "version":
                    _console.AddText($" {Application.productName} : {Application.version} ");
                    break;
                case "state":
                    _console.AddText($" 当前状态 : {CurrentState.ToString()} ");
                    break;
                case "exit":
                    if (CurrentState.ToString() != typeof(CommonState).ToString())
                    {
                        SwitchToState(new CommonState(_console,this));
                    }
                    break;
                
                default:
                    break;
            }
            CurrentState.HandleInput(input);
        }

        public void UpdateState()
        {
            CurrentState.Update();
        }
        
        
    }
}