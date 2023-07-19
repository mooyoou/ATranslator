public interface IState
{
    void Enter();
    void Exit();
    void HandleInput(string input);
}