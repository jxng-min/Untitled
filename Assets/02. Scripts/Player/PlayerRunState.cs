using UnityEngine;

public class PlayerRunState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.FallTime = 0f;

            m_player_ctrl.Animator.SetBool("IsMove", true);
            m_player_ctrl.Animator.SetBool("LShift", true);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        if(Input.GetKey(KeyCode.E))
        {
            m_player_ctrl.ChangeState(PlayerState.BLOCK);
        }
        else
        {
            m_player_ctrl.Jump(m_player_ctrl.JumpPower);

            m_player_ctrl.Attack();

            if(m_player_ctrl.FallTime > 0.3f)
            {
                m_player_ctrl.ChangeState(PlayerState.JUMPING);
            }
            
            if(m_player_ctrl.Direction.magnitude > 0f)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    m_player_ctrl.Move(7f);

                    m_player_ctrl.Animator.SetFloat("MoveZ", m_player_ctrl.Direction.z);
                    m_player_ctrl.Animator.SetFloat("MoveX", m_player_ctrl.Direction.x);
                }
                else
                {
                    m_player_ctrl.ChangeState(PlayerState.WALK);
                }
            }
            else
            {
                m_player_ctrl.ChangeState(PlayerState.IDLE);
            }
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.SetBool("IsMove", false);
        m_player_ctrl.Animator.SetBool("LShift", false);
    }
}
