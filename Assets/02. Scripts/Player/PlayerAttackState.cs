using System.Collections;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetTrigger("Attack");
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Invoke("IdleWait", m_player_ctrl.Data.PlayerStat.Rate + 0.4f);
    }

    private void IdleWait()
    {
        if(Input.GetKey(KeyCode.E))
        {
            m_player_ctrl.ChangeState(PlayerState.BLOCK);
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

    public void ExecuteExit(PlayerCtrl sender)
    {

    }
}
