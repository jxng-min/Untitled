using UnityEngine;

public class PlayerJumpingState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetTrigger("Jumping");
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        m_player_ctrl.Move(5f);

        if(m_player_ctrl.FallTime > 0.3f)
        {
            m_player_ctrl.ChangeState(PlayerState.JUMPOUT);
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.ResetTrigger("Jumping");
    }
}
