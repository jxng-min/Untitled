using UnityEngine;

namespace Junyoung
{
    public class EnemyAttackState : MonoBehaviour,IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private PlayerCtrl m_player;
        private float m_atk_ani_length;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
            m_enemy_ctrl.Animator.SetTrigger("Attack");
            m_player = gameObject.GetComponent<PlayerCtrl>();
            m_atk_ani_length = m_enemy_ctrl.EnemyStat.AtkAniLength;
            m_player.GetDamage(m_enemy_ctrl.EnemyStat.AtkDamege);
            //Debug.Log($"플레이어가{m_enemy_ctrl.EnemyStat.AtkDamege}의 데미지 입음");
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if(m_atk_ani_length>=0)
            {
                m_atk_ani_length -= Time.deltaTime;
            }
            else
            {
                m_enemy_ctrl.ChangeState(EnemyState.READY);
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {
            m_enemy_ctrl.TotalAtkRate = m_enemy_ctrl.EnemyStat.AtkAniLength / m_enemy_ctrl.EnemyStat.AtkAniSpeed + m_enemy_ctrl.EnemyStat.AtkRate;
        }
    }
}