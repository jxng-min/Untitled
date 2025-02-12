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
            m_enemy_ctrl.EnemyStat.HP += Damage;
            var indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator).GetComponent<DamageIndicator>();
            indicator.Init(transform.position + Vector3.up * 3f, Damage, Damage < 0f ? Color.red : Color.green);
            if(m_enemy_ctrl.EnemyStat.HP<=0)
            {
                m_enemy_ctrl.ChangeState(EnemyState.DEAD);
            }
            else
            {
                //m_enemy_ctrl.Animator.SetTrigger("GetDamage");
                //StartCoroutine(GetAniLength());
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
            //m_enemy_ctrl.Animator.ResetTrigger("GetDamage");
        }
        /*
        public IEnumerator GetAniLength()
        {
            m_get_damage_ani_length = 1f;
            yield return new WaitForSeconds(0.1f);

            m_get_damage_ani_length = m_enemy_ctrl.GetAniLength("Get Damage") - 0.1f;
        }
        */
    }
}