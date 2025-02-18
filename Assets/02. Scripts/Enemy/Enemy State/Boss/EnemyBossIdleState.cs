using Junyoung;
using UnityEngine;

public class EnemyBossIdleState : EnemyIdleState
{
    public override void OnStateEnter(EnemyCtrl sender)
    {
        m_enemy_ctrl = sender;

    }

    public override void OnStateUpdate(EnemyCtrl sender)
    {
        m_enemy_ctrl.DetectPlayer();
    }
}
