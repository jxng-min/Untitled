using UnityEngine;

public class PlayerDamageState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetTrigger("Damaged");
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Invoke("IdleWait", 0.4f);
    }

    private void IdleWait()
    {
        if(m_player_ctrl.IsGround)
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
        }        
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.ResetTrigger("Damaged");
    }
}
