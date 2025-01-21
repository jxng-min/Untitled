using UnityEngine;

public class PlayerRunState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetBool("IsMove", true);
            m_player_ctrl.Animator.SetBool("LShift", true);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        m_player_ctrl.Jump(m_player_ctrl.JumpPower);

        if(m_player_ctrl.FallTime > 0.3f)
        {
            m_player_ctrl.ChangeState(PlayerState.JUMPING);
        }
        
        m_player_ctrl.Move(7f);

        m_player_ctrl.Animator.SetFloat("MoveZ", m_player_ctrl.Direction.z);
        m_player_ctrl.Animator.SetFloat("MoveX", m_player_ctrl.Direction.x);
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.SetBool("IsMove", false);
        m_player_ctrl.Animator.SetBool("LShift", false);
    }
}
