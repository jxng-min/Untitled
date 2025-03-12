using System.Collections;
using UnityEngine;

public class PlayerSkill5State : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    private GameObject m_effect;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;

        if(m_player_ctrl)
        {
            m_effect = m_player_ctrl.Skill5Effect;
            m_player_ctrl.IsAttack = true;
            m_player_ctrl.Animator.SetTrigger("Skill5");
            m_player_ctrl.UpdateMP(-10f);

            StartCoroutine(Skill5EffectBegin());
        }
    }
    
    public void Execute(PlayerCtrl sender)
    {
        Dead();
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.IsAttack = false;
        m_player_ctrl.Animator.ResetTrigger("Skill5");
    }

    private void Dead()
    {
        if(DataManager.Instance.Data.Stat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }

    private IEnumerator Skill5EffectBegin()
    {
        yield return new WaitForSeconds(1f);

        m_effect = Instantiate(m_player_ctrl.Skill5Effect, transform.position + Vector3.up * 2 + m_player_ctrl.Model.transform.forward * 2, m_player_ctrl.Model.transform.rotation);
        m_effect.GetComponent<Rigidbody>().AddForce(m_player_ctrl.Model.transform.forward * 15f, ForceMode.Impulse);
        StartCoroutine(EffectScaler());
    }

    private IEnumerator Skill5EffectEnd()
    {
        yield return new WaitForSeconds(3f);

        Destroy(m_effect);
    }

    private IEnumerator EffectScaler()
    {
        float elapsed_time = 0f;
        float target_time = 2.5f;

        Vector3 origin_scale = m_effect.transform.localScale;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            float t = elapsed_time / target_time;
            m_effect.transform.localScale = Vector3.Lerp(origin_scale, Vector3.one * 2.5f, t);

            yield return null;
        }

        elapsed_time = 0f;
        origin_scale = Vector3.one * 2.5f;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            float t = elapsed_time / target_time;
            m_effect.transform.localScale = Vector3.Lerp(origin_scale, Vector3.zero, t);

            yield return null;
        }

        yield return StartCoroutine(Skill5EffectEnd());
    }

    public void Skill5_Sound()
    {
        SoundManager.Instance.PlayEffect("Skill5 Start");
    }

    public void Skill5_End()
    {
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
