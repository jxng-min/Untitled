using UnityEngine;

namespace Junyoung
{
    public class EnemyBackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
            m_enemy_ctrl.Animator.SetBool("isBack", true);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            //���� �ڸ��� ����
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.Animator.SetBool("isBack", false);
        }
    }
}