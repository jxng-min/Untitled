using UnityEngine;

namespace Junyoung
{

    public class EnemyBowCtrl : EnemyCtrl
    {

        public override void InitComponent()
        {
            base.InitComponent();
            base.m_enemy_attack_state = gameObject.AddComponent<EnemyBowAttackState>();
        }

    }
}