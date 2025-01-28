using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetTrigger("IsAttack");
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && m_player_ctrl.IsGround)
        {
            m_player_ctrl.Animator.SetInteger("AttackCount", 0);
            return;
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
            Debug.Log("여기로?");
            m_player_ctrl.ChangeState(PlayerState.IDLE);
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.ResetTrigger("IsAttack");
        }
    }
}
