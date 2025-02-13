using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyGetDamageState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public float Damage { get; set; }

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
                m_enemy_ctrl.ChangeState(EnemyState.READY);
            }

        }

        public void OnStateUpdate(EnemyCtrl sender)
        {

        }

        public void OnStateExit(EnemyCtrl sender)
        {

        }

    }
}