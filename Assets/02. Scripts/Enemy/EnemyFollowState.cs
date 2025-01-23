using UnityEngine;

namespace Junyoung
{
    public class EnemyFollowState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
            m_enemy_ctrl.Animator.SetTrigger("PlayerFound");
            m_enemy_ctrl.Animator.SetBool("isFollowing", true);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            //플레이어 추격
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.Animator.SetBool("isFollowing", false);
        }
    }
}
