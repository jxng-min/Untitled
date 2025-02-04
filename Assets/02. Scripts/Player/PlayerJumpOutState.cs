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
            m_player_ctrl.Camera.Shaking(0.05f, 0.05f);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
        
        m_player_ctrl.Move(5f);
        
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.ResetTrigger("Jumping");
        m_player_ctrl.Animator.ResetTrigger("JumpOut");
    }

    private void Dead()
    {
        if(m_player_ctrl.Data.PlayerStat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }
}
