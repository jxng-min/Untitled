using UnityEngine;

namespace Junyoung
{
    public class EnemyReadyState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private GameObject m_player;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
                m_player = GameObject.FindWithTag("Player");
            }
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if (Vector3.Distance(m_player.transform.position, m_enemy_ctrl.transform.position) >= m_enemy_ctrl.CombatRadius)
            {
                m_enemy_ctrl.ChangeState(EnemyState.FOLLOW);
            }

            //���� ��Ÿ�� ����� ���� ������Ʈ�� ��ȯ �����ϰ� ����
        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }
        void OnDrawGizmos()
        {
            if (m_enemy_ctrl == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_enemy_ctrl.transform.position, m_enemy_ctrl.CombatRadius);
        }
    }
}