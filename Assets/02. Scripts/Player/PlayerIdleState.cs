using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.FallTime = 0f;

            m_player_ctrl.Animator.ResetTrigger("JumpIn");
            m_player_ctrl.Animator.ResetTrigger("Jumping");
            m_player_ctrl.Animator.ResetTrigger("JumpOut");
            m_player_ctrl.Animator.SetBool("IsMove", false);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();

        if(m_player_ctrl.IsGround)
        {
            if(Input.GetKey(KeyCode.E))
            {
                m_player_ctrl.ChangeState(PlayerState.BLOCK);
            }
            
            m_player_ctrl.Jump(m_player_ctrl.JumpPower);

            m_player_ctrl.Attack();

            m_player_ctrl.Skill1();
            m_player_ctrl.Skill2();
            m_player_ctrl.Skill3();

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

    private void Dead()
    {
        if(m_player_ctrl.Data.PlayerStat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {

    }
}
