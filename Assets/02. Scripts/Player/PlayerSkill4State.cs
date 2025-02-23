using System.Collections;
using UnityEngine;

public class PlayerSkill4State : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    private GameObject m_effect;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            Debug.Log("진입");
            m_player_ctrl.IsAttack = true;
            m_player_ctrl.Animator.SetTrigger("Skill4");
            m_player_ctrl.UpdateMP(-6f);

            StartCoroutine(Skill4EffectBegin());
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Dead();
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.IsAttack = false;
        m_player_ctrl.Animator.ResetTrigger("Skill4");

        StartCoroutine(Skill4EffectEnd());
    }

    private void Dead()
    {
        if(DataManager.Instance.Data.Stat.HP <= 0f)
        {
            m_player_ctrl.ChangeState(PlayerState.DEAD);
        }
    }

    private IEnumerator Skill4EffectBegin()
    {
        yield return new WaitForSeconds(1f);

        m_effect = Instantiate(m_player_ctrl.Skill4Effect, transform.position + Vector3.up, Quaternion.identity);
        StartCoroutine(EffectScaler());
        m_player_ctrl.Camera.Shaking(0.3f, 1.5f);
    }

    private IEnumerator Skill4EffectEnd()
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
            m_effect.transform.localScale = Vector3.Lerp(origin_scale, Vector3.one * 5, t);

            yield return null;
        }
    }

    public void Skill4_End()
    {
        m_player_ctrl.ChangeState(PlayerState.IDLE);
    }
}
