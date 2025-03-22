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
            Vector3 pos = RandomPos(m_enemy_ctrl.EnemySpawnData.SpawnVector.ToVector3(), m_enemy_ctrl.PatrolRange); // ���� ���� ������ ��ġ ����
            m_agent.SetDestination(pos);
            m_enemy_ctrl.Animator.SetBool("isPatrol", true);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            m_enemy_ctrl.DetectPlayer();

            if(m_agent.remainingDistance <= m_agent.stoppingDistance) // (���� ����)���� �Ÿ���, �������� ���� �������� ����� �ϴ� �Ÿ� ��
            {
                m_enemy_ctrl.ChangeState(EnemyState.IDLE); // ������ ������ IDLE�� ��ȯ
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_agent.ResetPath();
            m_enemy_ctrl.Animator.SetBool("isPatrol", false);
        }

        Vector3 RandomPos(Vector3 center, float range)//
        {
            Vector2 cir_pos = Random.insideUnitCircle * range;
            Vector3 rand_pos = new Vector3(center.x + cir_pos.x, center.y, center.z + cir_pos.y);

            NavMeshHit pos;

            for (int i = 0; i < 100; i++)
            {
                if (NavMesh.SamplePosition(rand_pos, out pos, 1.0f, NavMesh.AllAreas))
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
            Gizmos.DrawWireSphere(m_enemy_ctrl.EnemySpawnData.SpawnVector.ToVector3(), m_enemy_ctrl.PatrolRange);
        }


    }
}
