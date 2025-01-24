using UnityEngine;

public class PlayerStateContext
{
    private readonly PlayerCtrl m_player_ctrl;

    public IState<PlayerCtrl> Current { get; set; }

    public PlayerStateContext(PlayerCtrl player_ctrl)
    {
        m_player_ctrl = player_ctrl;
    }

    public void Transition(IState<PlayerCtrl> state)
    {
        Debug.Log(state);
        if(Current == state)
        {
            return;
        }

        Current?.ExecuteExit(m_player_ctrl);

        Current = state;
        Current?.ExecuteEnter(m_player_ctrl);
    }

    public void ExecuteUpdate()
    {
        if(!m_player_ctrl)
        {
            return;
        }

        Current.Execute(m_player_ctrl);
    }
}