using System.Collections.Generic;
using UnityEngine;

namespace Junyoung
{
    public class EnemyStateContext
    {
        private readonly EnemyCtrl m_enemy_ctrl;

        public IEnemyState<EnemyCtrl> NowState { get; set; }
        public EnemyState CurrentStateEnum { get; private set; }  // 현재 Enum 상태


        private Dictionary<EnemyState, IEnemyState<EnemyCtrl>> m_state_map;

        public EnemyStateContext(EnemyCtrl enemy_ctrl)
        {
            m_enemy_ctrl = enemy_ctrl;
            InitStates();
        }

        public void Transition(IEnemyState<EnemyCtrl> enemy_state)
        {
            if (NowState == enemy_state || m_enemy_ctrl.IsDead) { return; }
            NowState?.OnStateExit(m_enemy_ctrl);

            NowState = enemy_state;
            //CurrentStateEnum = m_state_map[enemy_state];
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

        private void InitStates()
        {
            m_state_map = new Dictionary<EnemyState, IEnemyState<EnemyCtrl>>()
            {
                { EnemyState.IDLE, new EnemyIdleState() },
                { EnemyState.IDLE, new EnemyBossIdleState() },
                { EnemyState.PATROL, new EnemyPatrolState() },
                { EnemyState.FOUNDPLAYER, new EnemyFoundPlayerState() },
                { EnemyState.FOLLOW, new EnemyFollowState() },
                { EnemyState.FOLLOW, new EnemyBossFollowState() },
                { EnemyState.BACK, new EnemyBackState() },
                { EnemyState.BACK, new EnemyBossBackState() },
                { EnemyState.READY, new EnemyReadyState() },
                { EnemyState.READY, new EnemyBowReadyState() },
                { EnemyState.READY, new EnemyBossReadyState() },
                { EnemyState.ATTACK, new EnemyAttackState() },
                { EnemyState.ATTACK, new EnemyBowAttackState() },
                { EnemyState.ATTACK, new EnemyBossAttackState() },
                { EnemyState.GETDAMAGE, new EnemyGetDamageState() },
                { EnemyState.DEAD, new EnemyDeadState() },
                { EnemyState.STUN, new EnemyStunState() }
            };
        }
    }




}