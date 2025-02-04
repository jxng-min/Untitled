using UnityEngine;

public interface IEnemyState<T>
{
    public void OnStateEnter(T sender);
    public void OnStateUpdate(T sender);
    public void OnStateExit(T sender);
}
