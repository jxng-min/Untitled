using System.Collections;
using UnityEngine;

public class PlayerSkill2State : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    private GameObject m_effect;
    private float m_origin_attack;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.IsAttack = true;
            m_player_ctrl.Animator.SetTrigger("Skill2");
            m_player_ctrl.Skill2Ready = false;
            m_player_ctrl.UpdateMP(-1f);
            m_origin_attack = DataManager.Instance.Data.Stat.ATK;

            StartCoroutine(Skill2EffectBegin());
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.IsAttack = false;
        m_player_ctrl.Animator.ResetTrigger("Skill2");
        StartCoroutine(Skill2EffectEnd());
    }

    private IEnumerator Skill2EffectBegin()
    {
        m_effect = Instantiate(m_player_ctrl.Skill2Effect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.3f);

        DataManager.Instance.Data.Stat.ATK = m_origin_attack + m_origin_attack * 0.5f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        m_player_ctrl.Weapon.Trail.colorGradient = gradient;
    }

    private IEnumerator Skill2EffectEnd()
    {
        yield return new WaitForSeconds(10f);

        DataManager.Instance.Data.Stat.ATK = m_origin_attack;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        m_player_ctrl.Weapon.Trail.colorGradient = gradient;
    }

    private void Dead()
    {
        if(DataManager.Instance.Data.Stat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }

    public void Skill2_End()
    {
        Destroy(m_effect);
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
