using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyAttackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        protected EnemyCtrl m_enemy_ctrl;
        protected PlayerCtrl m_player_ctrl;
        protected GameObject m_player;
        protected float m_atk_ani_length;

        public virtual void OnStateEnter(EnemyCtrl sender)
        {
            if(m_enemy_ctrl==null)
            {
                m_enemy_ctrl = sender;

                m_player = m_enemy_ctrl.Player;
                m_player_ctrl = m_player.GetComponent<PlayerCtrl>();

            }

            m_enemy_ctrl.Animator.SetTrigger("Attack");
            m_enemy_ctrl.IsHitting= false;
            StartCoroutine(GetAniLength("Attack"));
            m_enemy_ctrl.AttackDelay = 0;
        }
        public virtual void OnStateUpdate(EnemyCtrl sender)
        {
            if (m_atk_ani_length>=0)
            {
                m_atk_ani_length -= Time.deltaTime;
            }
            else
            {
                m_enemy_ctrl.ChangeState(EnemyState.READY);
            }

            if (m_enemy_ctrl.IsHit && m_player_ctrl != null && !m_enemy_ctrl.IsHitting)
            {
                m_player_ctrl.UpdateHP(-m_enemy_ctrl.EnemyStat.AtkDamege);
                m_enemy_ctrl.IsHitting = true;
            }
        }
        public virtual void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.IsHit = false;
        }
        protected IEnumerator GetAniLength(string ani_name) // Attack Ʈ���Ű� ȣ�� ������ �����̰� �־ StateInfo�� �ִϸ��̼� ��ȯ ���� ȣ��Ǵ� ���� ������ ���
        {
            m_atk_ani_length = 1f; // ��� �ð����� OnStateUpdate�� ���ؼ� READY state�� ��ȯ���� �ʱ� ����
            yield return new WaitForSeconds(0.1f); // �ణ�� ��� �ð��� �־� �ִϸ��̼� ���� ��ȯ�� ��ٸ�          
            m_atk_ani_length = m_enemy_ctrl.GetAniLength(ani_name);

            if (m_enemy_ctrl.TotalAtkRate == 0)
                m_enemy_ctrl.TotalAtkRate = m_atk_ani_length + m_enemy_ctrl.EnemyStat.AtkRate - 0.1f;

        }
    }
}