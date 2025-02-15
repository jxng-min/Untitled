using UnityEngine;
using UnityEngine.AI;

namespace Junyoung
{
    public class EnemyBackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private NavMeshAgent m_agent;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
                m_agent = m_enemy_ctrl.Agent;
            }
            m_enemy_ctrl.Animator.SetBool("isBack", true);
            m_agent.speed *= 1.4f; // 복귀시 이동속도 증가
            m_agent.SetDestination(m_enemy_ctrl.EnemySpawnData.SpawnTransform.position);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                m_enemy_ctrl.ChangeState(EnemyState.IDLE);
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_agent.speed /= 1.4f;
            m_enemy_ctrl.Animator.SetBool("isBack", false);
        }
    }
}