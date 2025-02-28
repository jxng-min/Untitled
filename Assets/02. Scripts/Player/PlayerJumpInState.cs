using UnityEngine;

public class PlayerJumpInState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            SoundManager.Instance.PlayEffect("Player Jump");
            m_player_ctrl.Animator.SetTrigger("JumpIn");
        }  
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
        
        m_player_ctrl.Move(5f);

        if(m_player_ctrl.FallTime > 0.3f)
        {
            m_player_ctrl.ChangeState(PlayerState.JUMPING);
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
        m_player_ctrl.Animator.ResetTrigger("JumpIn");
    }
}
