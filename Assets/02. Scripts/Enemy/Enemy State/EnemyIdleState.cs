using UnityEngine;

namespace Junyoung
{
    public class EnemyIdleState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        protected EnemyCtrl m_enemy_ctrl;
        private float m_idling_time;

        public virtual void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl= sender;
            m_enemy_ctrl.Animator.SetBool("isPatrol", false);

            m_idling_time = UnityEngine.Random.Range(2.0f, 7.0f); // invoke�� �ϸ� ���߿� follow state�� ��ȯ �Ǿ �״�� ������
        }
        public virtual void OnStateUpdate(EnemyCtrl sender)
        {
            m_enemy_ctrl.DetectPlayer();

            m_idling_time -= Time.deltaTime;
            if (m_idling_time<=0)
            {
                m_enemy_ctrl.ChangeState(EnemyState.PATROL);             
            }


        }
        public virtual void OnStateExit(EnemyCtrl sender)
        {

        }

        
    }
}