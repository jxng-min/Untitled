using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    public int ComboIndex { get; set; }
    public bool ComboEnable { get; set; }
    public bool ComboExist { get; set; }

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
        if(ComboIndex >= 2)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) == false)
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

        if(m_player_ctrl.IsAttack)
        {
            return;
        }

        m_player_ctrl.IsAttack = true;
        m_player_ctrl.Animator.SetBool("IsAttack", m_player_ctrl.IsAttack);
    }

    public void Combo_Enable()
    {
        ComboEnable = true;

        if(ComboIndex < 3)
        {
            m_player_ctrl.Weapon.Use();
        }
    }

    public void Combo_Disable()
    {
        ComboEnable = false;
    }

    public void Combo_Exist()
    {
        if(!ComboExist)
        {
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