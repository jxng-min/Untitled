using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyAttackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private PlayerCtrl m_player;
        private float m_atk_ani_length = 1f;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
            m_enemy_ctrl.Animator.SetTrigger("Attack");
            m_player = gameObject.GetComponent<PlayerCtrl>();
                                 
            StartCoroutine(GetAniLength());
            m_enemy_ctrl.AttackDelay = 0;

            //m_player.GetDamage(m_enemy_ctrl.EnemyStat.AtkDamege);
            Debug.Log($"플레이어가{m_enemy_ctrl.EnemyStat.AtkDamege}의 데미지 입음");
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
        }
        public void OnStateExit(EnemyCtrl sender)
        {
        }

        private IEnumerator GetAniLength() // Attack 트리거가 호출 됐지만 딜레이가 있어서 StateInfo가 애니메이션 전환 전에 호출되는 문제 때문에 사용
        {
            yield return new WaitForSeconds(0.1f); // 약간의 대기 시간을 주어 애니메이션 상태 전환을 기다림          
            m_atk_ani_length = m_enemy_ctrl.GetAniLength("Attack");

            Debug.Log(m_atk_ani_length);
            if (m_enemy_ctrl.TotalAtkRate == 0)
                m_enemy_ctrl.TotalAtkRate = m_atk_ani_length + m_enemy_ctrl.EnemyStat.AtkRate - 0.1f;

            Debug.Log(m_enemy_ctrl.TotalAtkRate);

        }
    }
}