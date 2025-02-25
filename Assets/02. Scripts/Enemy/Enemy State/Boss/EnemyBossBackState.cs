using UnityEngine;

namespace Junyoung
{
    public class EnemyBossBackState : EnemyBackState
    {
        public override void OnStateEnter(EnemyCtrl sender)
        {
            base.OnStateEnter(sender);
            (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = true;
            StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).Regeneration(5f));
        }

        public override void OnStateExit(EnemyCtrl sender)
        {
            base.OnStateExit(sender);
            (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = false;
        }
    }
}