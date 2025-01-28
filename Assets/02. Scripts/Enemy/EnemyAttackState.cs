using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyAttackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private PlayerCtrl m_player_ctrl;
        private float m_atk_ani_length;
        private bool m_is_hitting;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if(m_enemy_ctrl==null)
            {
                m_enemy_ctrl = sender;

                GameObject[] m_players = GameObject.FindGameObjectsWithTag("Player");

                foreach(GameObject p in m_players ) // Player�±׸� ���� �ֻ��� ������Ʈ�� ã��
                {
                    if (p.transform.parent == null)
                    {
                        m_player_ctrl = p.GetComponent<PlayerCtrl>();
                    }
                }

            }
            m_enemy_ctrl.Animator.SetTrigger("Attack");
            m_is_hitting = false;
            StartCoroutine(GetAniLength());
            m_enemy_ctrl.AttackDelay = 0;
            //Debug.Log($"�÷��̾{m_enemy_ctrl.EnemyStat.AtkDamege}�� ������ ����");

        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if(m_atk_ani_length>=0)
            {
                m_atk_ani_length -= Time.deltaTime;
            }
            else
            {
                m_enemy_ctrl.ChangeState(EnemyState.READY);
            }

            if (m_enemy_ctrl.IsHit && m_player_ctrl != null && !m_is_hitting)
            {
                m_player_ctrl.GetDamage(m_enemy_ctrl.EnemyStat.AtkDamege);
                m_is_hitting = true;
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.IsHit = false;
        }

        private IEnumerator GetAniLength() // Attack Ʈ���Ű� ȣ�� ������ �����̰� �־ StateInfo�� �ִϸ��̼� ��ȯ ���� ȣ��Ǵ� ���� ������ ���
        {
            m_atk_ani_length = 1f; // ��� �ð����� OnStateUpdate�� ���ؼ� READY state�� ��ȯ���� �ʱ� ����
            yield return new WaitForSeconds(0.1f); // �ణ�� ��� �ð��� �־� �ִϸ��̼� ���� ��ȯ�� ��ٸ�          
            m_atk_ani_length = m_enemy_ctrl.GetAniLength("Attack");

            if (m_enemy_ctrl.TotalAtkRate == 0)
                m_enemy_ctrl.TotalAtkRate = m_atk_ani_length + m_enemy_ctrl.EnemyStat.AtkRate - 0.1f;

        }
    }
}