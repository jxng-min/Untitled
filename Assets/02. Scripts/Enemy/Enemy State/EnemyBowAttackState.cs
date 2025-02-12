using System.Collections;
using UnityEngine;

namespace Junyoung
{

    public class EnemyBowAttackState : EnemyAttackState
    {
        ArrowFactory m_arrow_factory;
        EnemyBowCtrl m_enemy_bow_ctrl;

        public override void OnStateEnter(EnemyCtrl sender)
        {
            base.OnStateEnter(sender);
            
            if(!m_arrow_factory) 
            {
                m_arrow_factory = gameObject.GetComponent<ArrowFactory>();
                m_enemy_bow_ctrl = gameObject.GetComponent<EnemyBowCtrl>();
            }

            ArrowCtrl arrow = m_arrow_factory.SpawnArrow(m_enemy_bow_ctrl.m_arrow_spawn_pos);
            FireArrow(arrow);
        }

        public void FireArrow(ArrowCtrl arrow)
        {
            Vector3 dir = (m_player.transform.position - gameObject.transform.position).normalized;
            arrow.GetComponent<Rigidbody>().AddForce(dir * 2f, ForceMode.Impulse);
        }
    }
}