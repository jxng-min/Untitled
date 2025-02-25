using Junyoung;
using UnityEngine;

public class EnemyBossIdleState : EnemyIdleState
{
    public override void OnStateEnter(EnemyCtrl sender)
    {
        m_enemy_ctrl = sender;
        (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = true;
        StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).Regeneration(15f));
    }

    public override void OnStateUpdate(EnemyCtrl sender)
    {
        m_enemy_ctrl.DetectPlayer();
    }

    public override void OnStateExit(EnemyCtrl sender)
    {
        base.OnStateExit(sender);
        (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = false;
    }
}
