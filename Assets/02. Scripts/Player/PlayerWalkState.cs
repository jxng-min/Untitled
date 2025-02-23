using UnityEngine;

public class PlayerWalkState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.FallTime = 0f;

            m_player_ctrl.Animator.SetBool("IsMove", true);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
        
        if(Input.GetKey(KeyCode.Q))
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
                    m_player_ctrl.ChangeState(PlayerState.RUN);
                }
                else
                {
                    m_player_ctrl.Move(5f);

                    m_player_ctrl.Animator.SetFloat("MoveZ", m_player_ctrl.Direction.z);
                    m_player_ctrl.Animator.SetFloat("MoveX", m_player_ctrl.Direction.x);
                }
            }
            else
            {
                m_player_ctrl.ChangeState(PlayerState.IDLE);
            }
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
        m_player_ctrl.Animator.SetBool("IsMove", false);
    }
}