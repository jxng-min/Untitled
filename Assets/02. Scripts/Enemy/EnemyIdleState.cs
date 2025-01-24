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

            m_idling_time = UnityEngine.Random.Range(2.0f, 7.0f); // invoke로 하면 도중에 follow state로 전환 되어도 그대로 동작함
            Debug.Log($"Idle State Entered. Idling Time: {m_idling_time}");
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            m_enemy_ctrl.DetectPlayer();

            m_idling_time -= Time.deltaTime;
            if (m_idling_time<=0)
            {
                Debug.Log($"idle 타임 종료 PATROL state로 전환");
                m_enemy_ctrl.ChangeState(EnemyState.PATROL);             
            }


        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }

        
    }
}