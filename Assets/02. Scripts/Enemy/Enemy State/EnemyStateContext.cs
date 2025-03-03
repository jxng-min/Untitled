using UnityEngine;

namespace Junyoung
{
    public class EnemyStateContext
    {
        private readonly EnemyCtrl m_enemy_ctrl;

        public IEnemyState<EnemyCtrl> NowState { get; set; }

        public EnemyStateContext(EnemyCtrl enemy_ctrl)
        {
            m_enemy_ctrl = enemy_ctrl;
        }

        public void Transition(IEnemyState<EnemyCtrl> enemy_state)
        {
            if (NowState == enemy_state || m_enemy_ctrl.IsDead) { return; }
            Debug.Log($"{enemy_state.ToString()}");
            NowState?.OnStateExit(m_enemy_ctrl);

            NowState = enemy_state;
            NowState?.OnStateEnter(m_enemy_ctrl);
            
        }


        public void OnStateUpdate()
        {
            if (!m_enemy_ctrl)
            {
                return;
            }

            NowState.OnStateUpdate(m_enemy_ctrl);
        }
    }

}