using UnityEngine;

namespace Junyoung
{
    public class EnemyIdleState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private float m_idling_time;

        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl= sender;
            m_enemy_ctrl.Animator.SetBool("isPatrol", false);

            m_idling_time = UnityEngine.Random.Range(2.0f, 7.0f); // invoke�� �ϸ� ���߿� follow state�� ��ȯ �Ǿ �״�� ������
            Debug.Log($"Idle State Entered. Idling Time: {m_idling_time}");
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            m_enemy_ctrl.DetectPlayer();

            m_idling_time -= Time.deltaTime;
            if (m_idling_time<=0)
            {
                Debug.Log($"idle Ÿ�� ���� PATROL state�� ��ȯ");
                m_enemy_ctrl.ChangeState(EnemyState.PATROL);             
            }


        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }

        
    }
}