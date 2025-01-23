using UnityEngine;

namespace Junyoung
{
    public class EnemyIdleState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;

        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl= sender;
            m_enemy_ctrl.Animator.SetBool("isPatrol", false);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            //�÷��̾� Ž��
        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }
    }
}