using UnityEngine;

namespace Junyoung
{
    public class EnemyReadyState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            //공격 쿨타임 대기후 공격 스테이트로 전환 가능하게 변경
        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }
    }
}