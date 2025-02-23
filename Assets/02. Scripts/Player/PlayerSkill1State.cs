using System.Collections;
using UnityEngine;

public class PlayerSkill1State : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    private GameObject m_effect;
    
    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.IsAttack = true;
            m_player_ctrl.Animator.SetTrigger("Skill1");
            m_player_ctrl.UpdateMP(-6f);
            StartCoroutine(Skill1EffectBegin());
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.IsAttack = false;
        m_player_ctrl.Animator.ResetTrigger("Skill1");
        StartCoroutine(Skill1EffectEnd());
    }

    private IEnumerator Skill1EffectBegin()
    {
        yield return new WaitForSeconds(1f);

        m_effect = Instantiate(m_player_ctrl.Skill1Effect, transform.position, Quaternion.identity);
        m_player_ctrl.Camera.Shaking(0.3f, 0.6f);
    }

    private IEnumerator Skill1EffectEnd()
    {
        yield return new WaitForSeconds(5f);

        Destroy(m_effect);
    }

    private void Dead()
    {
        if(DataManager.Instance.Data.Stat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }

    public void Skill1_End()
    {
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
