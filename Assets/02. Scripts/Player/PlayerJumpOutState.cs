using UnityEngine;

public class PlayerJumpOutState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetTrigger("JumpOut");
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        m_player_ctrl.Move(5f);
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.ResetTrigger("JumpOut");
    }
}
