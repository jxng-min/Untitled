using UnityEngine;

public class PlayerBlockState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetBool("IsBlock", true);
            m_player_ctrl.IsBlock = true;
            
            if(m_player_ctrl.Direction.magnitude > 0f)
            {
                m_player_ctrl.Animator.SetBool("IsMove", true);
            }
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        if(m_player_ctrl.IsGround)
        {
            if(Input.GetKey(KeyCode.Q))
            {
                m_player_ctrl.ChangeState(PlayerState.BLOCK);

                if(m_player_ctrl.FallTime > 0.4f)
                {
                    m_player_ctrl.ChangeState(PlayerState.JUMPING);
                }
                
                if(m_player_ctrl.Direction.magnitude > 0f)
                {
                    m_player_ctrl.Move(5f);

                    m_player_ctrl.Animator.SetBool("IsMove", true);
                    m_player_ctrl.Animator.SetFloat("MoveZ", m_player_ctrl.Direction.z);
                    m_player_ctrl.Animator.SetFloat("MoveX", m_player_ctrl.Direction.x);
                }
                else
                {
                    m_player_ctrl.Animator.SetBool("IsMove", false);
                }
            }
            else
            {
                if(m_player_ctrl.FallTime > 0.4f)
                {
                    m_player_ctrl.ChangeState(PlayerState.JUMPING);
                }

                if(m_player_ctrl.Direction.magnitude > 0f)
                {
                    if(Input.GetKey(KeyCode.LeftShift))
                    {
                    m_player_ctrl.ChangeState(PlayerState.RUN);
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
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.SetBool("IsBlock", false);
        m_player_ctrl.IsBlock = false;

        m_player_ctrl.Animator.SetBool("IsMove", false);
    }
}
