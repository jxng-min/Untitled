using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyAttackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private PlayerCtrl m_player_ctrl;
        private GameObject m_player;
        private GameObject m_enemy;
        private float m_atk_ani_length;
        private bool m_is_hitting;
        private float m_rotation_speed = 5f;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if(m_enemy_ctrl==null)
            {
                m_enemy_ctrl = sender;
                m_enemy = m_enemy_ctrl.gameObject;
                m_player = m_enemy_ctrl.Player;
                m_player_ctrl = m_player.GetComponent<PlayerCtrl>();
            }
            m_enemy_ctrl.Animator.SetTrigger("Attack");
            m_is_hitting = false;
            StartCoroutine(GetAniLength());
            m_enemy_ctrl.AttackDelay = 0;

            //Debug.Log($"플레이어가{m_enemy_ctrl.EnemyStat.AtkDamege}의 데미지 입음");
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if (m_atk_ani_length>=0)
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



        private IEnumerator GetAniLength() // Attack 트리거가 호출 됐지만 딜레이가 있어서 StateInfo가 애니메이션 전환 전에 호출되는 문제 때문에 사용
        {
            m_atk_ani_length = 1f; // 대기 시간동안 OnStateUpdate에 의해서 READY state로 전환되지 않기 위해
            yield return new WaitForSeconds(0.1f); // 약간의 대기 시간을 주어 애니메이션 상태 전환을 기다림          
            m_atk_ani_length = m_enemy_ctrl.GetAniLength("Attack");

            if (m_enemy_ctrl.TotalAtkRate == 0)
                m_enemy_ctrl.TotalAtkRate = m_atk_ani_length + m_enemy_ctrl.EnemyStat.AtkRate - 0.1f;

        }
    }
}