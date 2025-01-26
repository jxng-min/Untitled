using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public bool ComboExist { get; set; }
    public bool ComboEnable { get; set; }
    public int ComboIndex { get; set; }

    public void ExecuteEnter(PlayerCtrl sender)
    {
        Debug.Log("공격 시작");
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.IsAttack = true;
            m_player_ctrl.Animator.SetBool("IsAttack", m_player_ctrl.IsAttack);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) == false)
        {
            return;
        }

        if(ComboEnable)
        {
            ComboEnable = false;
            ComboExist = true;

            return;
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
            return;
        }

        ComboExist = false;
        
        ComboIndex++;
        m_player_ctrl.Animator.SetTrigger("NextCombo");
    }


    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.IsAttack = false;
        m_player_ctrl.Animator.SetBool("IsAttack", m_player_ctrl.IsAttack);
        ComboIndex = 0;
    }
}
