public interface IState<T>
{
    void ExecuteEnter(T sender);
    void ExecuteUpdate(T sender);
    void ExecuteExit(T sender);
}
