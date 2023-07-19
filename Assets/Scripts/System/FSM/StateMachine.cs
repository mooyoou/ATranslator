
using UnityEngine;

public abstract class StateMachine : MonoBehaviour,IStateMachine
{
    protected IState CurrentState;

    public StateMachine(IState state = null)
    {
        if (state != null)
        {
            CurrentState = state;
            CurrentState.Enter();
        }
    }

    public virtual void ProcessInput(string input)
    {
        CurrentState.HandleInput(input);
    }

    public virtual void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}