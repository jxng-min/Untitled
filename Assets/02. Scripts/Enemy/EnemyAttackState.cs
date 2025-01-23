using UnityEngine;

namespace Junyoung
{
    public class EnemyAttackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
            m_enemy_ctrl.Animator.SetTrigger("Attack");
            //공격 함수 호출
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            
        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }
    }
}