using UnityEngine;

namespace Junyoung
{
    public class EnemyReadyState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            m_enemy_ctrl = sender;
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            //���� ��Ÿ�� ����� ���� ������Ʈ�� ��ȯ �����ϰ� ����
        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }
    }
}