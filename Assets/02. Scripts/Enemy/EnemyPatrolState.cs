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
            m_agent.stoppingDistance = 1f;
            Vector3 pos = RandomPos(m_enemy_ctrl.PatrolCenter.position, m_enemy_ctrl.PatrolRange); // ���� ���� ������ ��ġ ����
            m_agent.SetDestination(pos);
            Debug.Log($"PATROL state ���� ���� ��ġ ����");
            m_enemy_ctrl.Animator.SetBool("isPatrol", true);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            m_enemy_ctrl.DetectPlayer();

            if(m_agent.remainingDistance <= m_agent.stoppingDistance) // (���� ����)���� �Ÿ���, �������� ���� �������� ����� �ϴ� �Ÿ� ��
            {
                Debug.Log($"������ ���� IDLE�� ��ȯ");
                m_enemy_ctrl.ChangeState(EnemyState.IDLE); // ������ ������ IDLE�� ��ȯ
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_agent.ResetPath();
            m_enemy_ctrl.Animator.SetBool("isPatrol", false);
        }

        Vector3 RandomPos(Vector3 center, float range)
        {
            Vector3 randPos = center + Random.insideUnitSphere* range;
            randPos.y = center.y;
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
        private void OnDrawGizmosSelected()// PatrolCenter�� �������� PatrolRange �������� ���� �׸�
        {
            if (m_enemy_ctrl == null) return;
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(m_enemy_ctrl.PatrolCenter.position, m_enemy_ctrl.PatrolRange);
        }


    }
}
