using UnityEngine;

namespace Junyoung
{
    public class EnemyBossBackState : EnemyBackState
    {
        public override void OnStateEnter(EnemyCtrl sender)
        {
            Debug.Log("�齺����Ʈ ��");
            base.OnStateEnter(sender);
            (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = true;
            StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).Regeneration(5f));
        }

        public override void OnStateExit(EnemyCtrl sender)
        {
            Debug.Log("�齺����Ʈ �ƿ�");
            base.OnStateExit(sender);
            (m_enemy_ctrl as EnemyBossCtrl).IsNotCombating = false;
        }
    }
}