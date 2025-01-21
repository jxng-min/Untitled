public interface IState<T>
{
    public void ExecuteEnter(T sender);
    public void Execute(T sender);
    public void ExecuteExit(T sender);
}