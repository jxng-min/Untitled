using UnityEngine;

public class PlayerIdle : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetBool("IsMove", false);
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {

    }

    public void ExecuteUpdate(PlayerCtrl sender)
    {

    }
}
