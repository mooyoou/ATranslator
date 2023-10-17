
using System.Runtime.InteropServices.ComTypes;
using DebugSystem;
using UnityEngine;

public abstract class StateMachine
{
    protected IState CurrentState;

    public virtual void ProcessInput(string input)
    {
        CurrentState.HandleInput(input);
    }
    
    public virtual void SwitchToState(IState state)
    {
        CurrentState.Exit();
        
        CurrentState = state;
        state.Enter();
        
    }
    
    
}