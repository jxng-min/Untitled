using UnityEngine;

public class PlayerJumpingState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            if(!m_player_ctrl.IsGround)
            {
                m_player_ctrl.Animator.SetTrigger("Jumping");
            }
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
        
        m_player_ctrl.Move(5f);

        if(m_player_ctrl.IsGround)
        {
            m_player_ctrl.ChangeState(PlayerState.JUMPOUT);
        }
    }

    private void Dead()
    {
        if(DataManager.Instance.Data.Stat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.ResetTrigger("Jumping");
    }
}
