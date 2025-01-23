using UnityEngine;

namespace Junyoung
{
    public class EnemyGetDamageState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
            m_enemy_ctrl.Animator.SetTrigger("GetDamage");
            //ü�� ����
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {

        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }
    }
}