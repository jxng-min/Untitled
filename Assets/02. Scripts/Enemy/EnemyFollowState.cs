using UnityEngine;
using UnityEngine.AI;

namespace Junyoung
{
    public class EnemyFollowState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private NavMeshAgent m_agent;
        private bool m_can_follow;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
                m_agent = m_enemy_ctrl.Agent;
            }
            m_can_follow = false;

            m_enemy_ctrl.Animator.SetTrigger("PlayerFound");
            m_enemy_ctrl.Animator.SetBool("isFollowing", true);
            Invoke("FollowStart", 2f);
            m_agent.stoppingDistance = 3f;
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if (!m_can_follow) return;

            Collider[] colliders = Physics.OverlapSphere(m_enemy_ctrl.transform.position, m_enemy_ctrl.CombatRadius);

            foreach(Collider col in colliders)
            {
                if(col.CompareTag("Player"))
                {
                    m_agent.SetDestination(col.transform.position);
                    Debug.Log($"{m_agent.destination}");
                }
            }

            if (m_agent.pathPending) return;

            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                m_enemy_ctrl.ChangeState(EnemyState.READY);
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.Animator.SetBool("isFollowing", false);
        }

        void OnDrawGizmos()
        {
            if (m_enemy_ctrl == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_enemy_ctrl.transform.position, m_enemy_ctrl.CombatRadius);
        }

        void FollowStart()
        {
            m_can_follow= true;
        }

    }
}
