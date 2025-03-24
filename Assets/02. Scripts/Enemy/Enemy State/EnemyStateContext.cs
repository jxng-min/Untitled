using System.Collections.Generic;
using UnityEngine;

namespace Junyoung
{
    public class EnemyStateContext
    {
        private readonly EnemyCtrl m_enemy_ctrl;

        public IEnemyState<EnemyCtrl> NowState { get; set; }
        public EnemyState NowStateEnum { get; private set; }  // 현재 Enum 상태


        private Dictionary<IEnemyState<EnemyCtrl>,EnemyState > m_state_map;

        public EnemyStateContext(EnemyCtrl enemy_ctrl)
        {
            m_enemy_ctrl = enemy_ctrl;          
        }

        public void Transition(IEnemyState<EnemyCtrl> enemy_state)
        {
            if (NowState == enemy_state || m_enemy_ctrl.IsDead) { return; }
            NowState?.OnStateExit(m_enemy_ctrl);

            if(m_state_map == null)
            {
                InitStates();
            }

            NowState = enemy_state;
            NowStateEnum = m_state_map[enemy_state];
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
            m_state_map = new Dictionary<IEnemyState<EnemyCtrl>, EnemyState >()
            {
                { m_enemy_ctrl.m_enemy_idle_state, EnemyState.IDLE  },
                { m_enemy_ctrl.m_enemy_patrol_state , EnemyState.PATROL },
                { m_enemy_ctrl.m_enemy_found_player_state , EnemyState.FOUNDPLAYER },
                { m_enemy_ctrl.m_enemy_follow_state , EnemyState.FOLLOW },
                { m_enemy_ctrl.m_enemy_back_state , EnemyState.BACK },
                { m_enemy_ctrl.m_enemy_ready_state , EnemyState.READY },
                { m_enemy_ctrl.m_enemy_attack_state , EnemyState.ATTACK },
                { m_enemy_ctrl.m_enemy_get_damage_state , EnemyState.GETDAMAGE },
                { m_enemy_ctrl.m_enemy_dead_state , EnemyState.DEAD },
                { m_enemy_ctrl.m_enemy_stun_state , EnemyState.STUN }
            };
            Debug.Log($"매핑 초기화 완료  ");
        }
    }




}