using UnityEngine;

namespace Junyoung
{
    public class EnemyStateContext
    {
        private readonly EnemyCtrl m_enemy_ctrl;

        private IEnemyState<EnemyCtrl> m_now_state { get; set; }

        public EnemyStateContext(EnemyCtrl enemy_ctrl)
        {
            m_enemy_ctrl = enemy_ctrl;
        }

        public void Transition(IEnemyState<EnemyCtrl> enemy_state)
        {
            if (m_now_state == enemy_state) { return; }

            m_now_state?.OnStateExit(m_enemy_ctrl);

            m_now_state = enemy_state;
            m_now_state?.OnStateEnter(m_enemy_ctrl);
        }

        public void OnStateUpdate()
        {
            if (!m_enemy_ctrl)
            {
                return;
            }

            m_now_state.OnStateUpdate(m_enemy_ctrl);
        }
    }

}