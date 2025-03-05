using System.Collections;
using UnityEngine;

public class PlayerSkill3State : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    private GameObject m_effect;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.IsAttack = true;
            m_player_ctrl.Animator.SetTrigger("Skill3");
            m_player_ctrl.UpdateMP(-3f);

            StartCoroutine(Skill3EffectBegin());
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.IsAttack = false;
        m_player_ctrl.Animator.ResetTrigger("Skill3");
        StartCoroutine(Skill3EffectEnd());
    }

    private IEnumerator Skill3EffectBegin()
    {
        yield return new WaitForSeconds(1.1f);

        m_effect = Instantiate(m_player_ctrl.Skill3Effect, transform.position, Quaternion.identity);
        m_player_ctrl.Camera.Shaking(0.3f, 0.2f);

        SoundManager.Instance.PlayEffect("Skill1 E1");
    }

    private IEnumerator Skill3EffectEnd()
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(m_effect);
    }

    private void Dead()
    {
        if(DataManager.Instance.Data.Stat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }

    public void Skill3_Sound()
    {
        SoundManager.Instance.PlayEffect("Skill1 Start");
    }

    public void Skill3_End()
    {
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
