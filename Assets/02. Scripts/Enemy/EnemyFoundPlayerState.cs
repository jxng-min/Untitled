using Junyoung;
using UnityEngine;

namespace Junyoung
{
    public class EnemyFoundPlayerState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
            }
            m_enemy_ctrl.Animator.SetTrigger("FoundPlayer");
            Invoke("StartFollow", 2f);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {

        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.Animator.ResetTrigger("FoundPlayer");
        }

        void StartFollow()
        {
            m_enemy_ctrl.ChangeState(EnemyState.FOLLOW);
        }
    }
}