using UnityEngine;

namespace Junyoung
{
    public class EnemyReadyState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        protected EnemyCtrl m_enemy_ctrl;
        protected GameObject m_player;
        

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
            m_enemy_ctrl.DetectPlayer();

            PlayerDeadCheck();

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

        protected virtual void PlayerDeadCheck()
        {
            if (m_player.GetComponent<PlayerCtrl>().StateContext.Current is PlayerDeadState)
            {
                m_enemy_ctrl.Animator.SetTrigger("CombatEnd");
                m_enemy_ctrl.ChangeState(EnemyState.IDLE);
            }
        }

        public void OnDrawGizmos()
        {
            if (m_enemy_ctrl == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_enemy_ctrl.transform.position, m_enemy_ctrl.EnemyStat.AtkRange);
        }
    }
}