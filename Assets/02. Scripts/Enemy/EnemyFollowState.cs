using UnityEngine;
using UnityEngine.AI;

namespace Junyoung
{
    public class EnemyFollowState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private GameObject m_player;
        private NavMeshAgent m_agent;
        private bool m_can_follow;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
                m_agent = m_enemy_ctrl.Agent;
                m_player = GameObject.FindWithTag("Player");
            }
            m_can_follow = false;

            m_enemy_ctrl.Animator.SetTrigger("PlayerFound");
            m_enemy_ctrl.Animator.SetBool("isFollowing", true);
            Invoke("FollowStart", 2f); // 플레이어 발견 애니메이션 종료 후 추격 시작
            m_agent.stoppingDistance = 3f;
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if (!m_can_follow) return;

            if (Vector3.Distance(m_player.transform.position,m_enemy_ctrl.transform.position) <= m_enemy_ctrl.FollowRadius)
            {
                m_agent.SetDestination(m_player.transform.position);
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
            Gizmos.DrawWireSphere(m_enemy_ctrl.transform.position, m_enemy_ctrl.FollowRadius);
        }

        void FollowStart()
        {
            m_can_follow= true;
        }

    }
}
