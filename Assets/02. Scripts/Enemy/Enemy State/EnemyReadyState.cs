using UnityEngine;

namespace Junyoung
{
    public class EnemyReadyState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private GameObject m_player;
        private float m_rotation_speed = 3.5f;

        public virtual void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
                m_player = m_enemy_ctrl.Player;
            }
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            LookPlayer();

            if (m_player.GetComponent<PlayerCtrl>().StateContext.Current is PlayerDeadState)
            {
                m_enemy_ctrl.Animator.SetTrigger("CombatEnd");
                m_enemy_ctrl.ChangeState(EnemyState.IDLE);             
            }

            if (Vector3.Distance(m_player.transform.position, m_enemy_ctrl.transform.position) >= m_enemy_ctrl.EnemyStat.AtkRange)
            {
                m_enemy_ctrl.ChangeState(EnemyState.FOLLOW);
            }

            if(m_enemy_ctrl.CanAtk && (Vector3.Distance(m_player.transform.position, m_enemy_ctrl.transform.position) <= m_enemy_ctrl.EnemyStat.AtkRange))
            {
                m_enemy_ctrl.ChangeState(EnemyState.ATTACK);
            }
        }
        public virtual void OnStateExit(EnemyCtrl sender)
        {

        }

        private void LookPlayer()
        {
            Vector3 dir = m_player.transform.position - m_enemy_ctrl.gameObject.transform.position;
            Quaternion player_rotation = Quaternion.LookRotation(dir);
            m_enemy_ctrl.gameObject.transform.rotation = Quaternion.Slerp(m_enemy_ctrl.gameObject.transform.rotation, player_rotation, m_rotation_speed * Time.deltaTime);
        }

        public void OnDrawGizmos()
        {
            if (m_enemy_ctrl == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_enemy_ctrl.transform.position, m_enemy_ctrl.EnemyStat.AtkRange);
        }
    }
}