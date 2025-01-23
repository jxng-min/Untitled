using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.ResetTrigger("JumpIn");
            m_player_ctrl.Animator.ResetTrigger("Jumping");
            m_player_ctrl.Animator.ResetTrigger("JumpOut");
            m_player_ctrl.Animator.SetBool("IsMove", false);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        if(Input.GetKey(KeyCode.E))
        {
            m_player_ctrl.ChangeState(PlayerState.BLOCK);
        }
        
        m_player_ctrl.Jump(m_player_ctrl.JumpPower);

        m_player_ctrl.Attack();

        if(m_player_ctrl.FallTime > 0.3f)
        {
            m_player_ctrl.ChangeState(PlayerState.JUMPING);
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {

    }
}
