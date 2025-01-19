using UnityEngine;

public class PlayerStateContext : MonoBehaviour
{
    private readonly PlayerCtrl m_player_ctrl;

    public IState<PlayerCtrl> State { get; set; }

    public PlayerStateContext(PlayerCtrl player_ctrl)
    {
        m_player_ctrl = player_ctrl;
    }

    public void Transition(IState<PlayerCtrl> state)
    {
        if(State != null)
        {
            State.ExecuteExit(m_player_ctrl);
        }

        State = state;
        State.ExecuteEnter(m_player_ctrl);
    }
}
