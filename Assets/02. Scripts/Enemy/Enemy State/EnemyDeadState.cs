using UnityEngine;

namespace Junyoung
{
    public class EnemyDeadState : MonoBehaviour , IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
            }             
            m_enemy_ctrl.Animator.SetTrigger("Dead");

            DropItem();

            QuestManager.Instance.UpdateKillQuestCount(m_enemy_ctrl.EnemySpawnData.EnemyType);
            DataManager.Instance.Data.EXP++;

            Invoke("Destroy", 4f);
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
        }
        public void OnStateExit(EnemyCtrl sender)
        {
        }

        private void DropItem()
        {
            var item_prefab = m_enemy_ctrl.m_drop_item_manager.DropRandomItem(m_enemy_ctrl.m_drop_item_bag);

            if (item_prefab)
            {
                var drop_item = Instantiate(item_prefab);
                drop_item.transform.position = new Vector3(m_enemy_ctrl.gameObject.transform.position.x, m_enemy_ctrl.gameObject.transform.position.y + 3f, m_enemy_ctrl.gameObject.transform.position.z);
                Rigidbody item_rigid = drop_item.GetComponent<Rigidbody>();

                Vector3 dir = new Vector3(Random.Range(-3f, 3f), 7f, Random.Range(-3f, 3f));
                item_rigid.AddForce(dir, ForceMode.Impulse);
            }

        }

        private void Destroy()
        {
            m_enemy_ctrl.ReturnToPool();
        }
    }
}