using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyStunState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public float StunTime;
        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)  
            {
                m_enemy_ctrl = sender;

            }
            m_enemy_ctrl.Animator.SetTrigger("GetStun");
            Invoke("ChangeToReady", StunTime);
        }

        public void OnStateUpdate(EnemyCtrl sender)
        {

        }

        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.Animator.SetTrigger("StunEnd");
        }

        public void ChangeToReady()
        {
            m_enemy_ctrl.ChangeState(EnemyState.READY) ;
        }
    }
}