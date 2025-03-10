using System.Collections;
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
            m_player_ctrl.IsAttack = true;
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

        if(ComboIndex < 3)
        {
            switch(ComboIndex)
            {
                case 0:
                    SoundManager.Instance.PlayEffect("Player Attack 1");
                    break;

                case 1:
                    SoundManager.Instance.PlayEffect("Player Attack 2");
                    break;
                
                case 2:
                    SoundManager.Instance.PlayEffect("Player Attack 3");
                    break;
            }
            
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
            StartCoroutine(WaitIdle());
            return;
        }

        ComboExist = false;

        ComboIndex++;
        m_player_ctrl.Animator.SetTrigger("NextCombo");
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        if(m_player_ctrl)
        {
            m_player_ctrl.IsAttack = false;
            m_player_ctrl.Animator.SetBool("IsAttack", m_player_ctrl.IsAttack);
            ComboIndex = 0;
        }
    }

    private IEnumerator WaitIdle()
    {
        yield return new WaitForSeconds(0.5f);

        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}