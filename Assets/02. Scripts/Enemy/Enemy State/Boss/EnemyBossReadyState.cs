using UnityEngine;

namespace Junyoung
{
    public class EnemyBossReadyState : EnemyReadyState
    {
        protected override void PlayerDeadCheck()
        {
            if (m_player.GetComponent<PlayerCtrl>().StateContext.Current is PlayerDeadState)
            {
                m_enemy_ctrl.ChangeState(EnemyState.BACK);
            }
        }
    }
}