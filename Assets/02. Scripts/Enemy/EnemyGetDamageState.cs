using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyGetDamageState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public float Damage { get; set; }
        private float m_get_damage_ani_length;

        public void OnStateEnter(EnemyCtrl sender)
        {
            if(m_enemy_ctrl == null)  // �ִϸ��̼� ��� �ӵ��� �ҷ��ͼ� ���� ���� �ð����� ����
            {
                m_enemy_ctrl = sender;

            }           
            m_enemy_ctrl.Animator.SetTrigger("GetDamage");
            StartCoroutine(GetAniLength());
            m_enemy_ctrl.EnemyStat.HP -= Damage;
            if(m_enemy_ctrl.EnemyStat.HP<=0)
            {
                m_enemy_ctrl.ChangeState(EnemyState.DEAD);
            }
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if(m_get_damage_ani_length >= 0)
            {
                m_get_damage_ani_length -= Time.deltaTime;
            }
            else
            {
                m_enemy_ctrl.ChangeState(EnemyState.READY);
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }

        public IEnumerator GetAniLength()
        {
            yield return new WaitForSeconds(0.1f);

            m_get_damage_ani_length = m_enemy_ctrl.GetAniLength("Get Damage") - 0.1f;
        }
    }
}