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
                m_player = m_enemy_ctrl.Player;
            }
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if (Vector3.Distance(m_player.transform.position, m_enemy_ctrl.transform.position) >= m_enemy_ctrl.CombatRadius)
            {
                m_enemy_ctrl.ChangeState(EnemyState.FOLLOW);
            }

            if(m_enemy_ctrl.CanAtk && (Vector3.Distance(m_player.transform.position, m_enemy_ctrl.transform.position) <= m_enemy_ctrl.EnemyStat.AtkRange))
            {
                m_enemy_ctrl.ChangeState(EnemyState.ATTACK);
            }
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