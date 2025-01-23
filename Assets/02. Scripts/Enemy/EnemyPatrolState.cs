using Junyoung;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Junyoung
{
    public class EnemyPatrolState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private NavMeshAgent m_agent;


        public void OnStateEnter(EnemyCtrl sender)
        {
            if(m_enemy_ctrl== null)
            {
                m_enemy_ctrl = sender;
                m_agent = m_enemy_ctrl.Agent;
            }
            m_enemy_ctrl.Animator.SetBool("isPatrol", true);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            //�÷��̾� Ž��


            if(m_agent.remainingDistance <= m_agent.stoppingDistance) // (���� ����)���� �Ÿ���, �������� ���� �������� ����� �ϴ� �Ÿ� ��
            {
                Vector3 pos;
                pos = RandomPos(m_enemy_ctrl.PatrolCenter.position, m_enemy_ctrl.PatrolRange);
                Debug.DrawRay(pos, Vector3.up, Color.green, 3.0f);
                m_agent.SetDestination(pos);
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.Animator.SetBool("isPatrol", false);
        }

        Vector3 RandomPos(Vector3 center, float range)
        {
            Vector3 randPos = center + Random.insideUnitSphere* range; // Random.insideUnitSphere �������� 1�� �� ���ο��� �������� ���� ������
            NavMeshHit pos;

            for (int i = 0; i < 100; i++)
            {
                if (NavMesh.SamplePosition(randPos, out pos, 1.0f, NavMesh.AllAreas))
                {
                    return pos.position;
                }
            }
            return m_agent.destination;
            
        }
        private void OnDrawGizmosSelected()
        {
            if (m_enemy_ctrl == null) return;

            // PatrolCenter�� �������� PatrolRange �������� ���� �׸�
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(m_enemy_ctrl.PatrolCenter.position, m_enemy_ctrl.PatrolRange);
        }
    }
}
