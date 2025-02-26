using UnityEngine;

namespace Junyoung
{
    public class EnemyBossBackState : EnemyBackState
    {
        public override void OnStateEnter(EnemyCtrl sender)
        {
            Debug.Log("백스테이트 인");
            base.OnStateEnter(sender);
            (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = true;
            StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).Regeneration(5f));
        }

        public override void OnStateExit(EnemyCtrl sender)
        {
            Debug.Log("백스테이트 아웃");
            base.OnStateExit(sender);
            (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = false;
        }
    }
}