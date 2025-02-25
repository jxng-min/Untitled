using UnityEngine;
using UnityEngine.AI;

namespace Junyoung
{
    public class EnemyFollowState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        protected EnemyCtrl m_enemy_ctrl;
        protected GameObject m_player;
        protected NavMeshAgent m_agent;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
                m_agent = m_enemy_ctrl.Agent;
                m_player = m_enemy_ctrl.Player;
            }         
            m_enemy_ctrl.Animator.SetBool("isFollowing", true);
            m_agent.stoppingDistance = m_enemy_ctrl.EnemyStat.AtkRange;
        }
        public virtual void OnStateUpdate(EnemyCtrl sender)
        {
            DistanceCheck();
            if (m_agent.pathPending) return;

            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                m_enemy_ctrl.ChangeState(EnemyState.READY);
            }
        }

        public virtual void DistanceCheck()
        {
            if (Vector3.Distance(m_enemy_ctrl.EnemySpawnData.SpawnTransform.position, m_enemy_ctrl.Player.transform.position) <= m_enemy_ctrl.EnemyStat.FollowRange)
            {
                m_agent.SetDestination(m_player.transform.position);
            }
            else
            {
                m_enemy_ctrl.ChangeState(EnemyState.BACK);
            }
        }

        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.Animator.SetBool("isFollowing", false);
            m_agent.stoppingDistance = 1f;
            m_agent.ResetPath();
        }

        public void OnDrawGizmos()
        {
            if (m_enemy_ctrl == null) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_enemy_ctrl.EnemySpawnData.SpawnTransform.position, m_enemy_ctrl.EnemyStat.FollowRange);
        }
    }
}
