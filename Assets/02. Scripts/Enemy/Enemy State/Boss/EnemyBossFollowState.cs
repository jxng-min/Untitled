﻿using UnityEngine;
using System.Collections;

namespace Junyoung
{
    public class EnemyBossFollowState : EnemyFollowState
    {
        public bool m_can_wrap = true;
        private float m_wrap_ani_length;
        private bool m_is_wrap = false;

        public override void OnStateUpdate(EnemyCtrl sender)
        {
            base.OnStateUpdate(sender);
            if (m_can_wrap && (m_enemy_ctrl as EnemyBossCtrl).IsPhaseTwo == true)
            {
                StartCoroutine(Wrap());
            }           
        }
        public IEnumerator Wrap()
        {
            StartCoroutine(WrapCoolDown());
            m_enemy_ctrl.Animator.SetTrigger("Wrap");
            m_enemy_ctrl.Agent.speed = 0f;
            StartCoroutine(GetAniLength());
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(m_wrap_ani_length);                

            var effect = (m_enemy_ctrl as EnemyBossCtrl).Effect(1,transform.position);
            StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).DestroyEffect(effect, 2f));

            m_enemy_ctrl.Agent.speed = m_enemy_ctrl.OriginEnemyStat.MoveSpeed;
            m_agent.Warp(m_player.transform.position);        
        }

        public IEnumerator WrapCoolDown()
        {
            m_can_wrap = false;
            yield return new WaitForSeconds(25f);
            m_can_wrap = true;
        }
        private IEnumerator GetAniLength() 
        {
            m_wrap_ani_length = 1f;
            yield return new WaitForSeconds(0.1f);        

            m_wrap_ani_length = m_enemy_ctrl.GetAniLength("Wrap") - 0.1f;
        }
    }
}