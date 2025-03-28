using UnityEngine;
using static UnityEditor.Progress;

namespace Junyoung
{
    public class EnemyDeadState : MonoBehaviour , IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private GameObject m_global_object;

        public void OnStateEnter(EnemyCtrl sender)
        {          
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
                m_global_object = GameObject.Find("[Global]");
            }

            m_enemy_ctrl.Animator.SetTrigger("Dead");

            if (m_enemy_ctrl.IsAlreadyDead)
            {
                Invoke("Destroy", 4f);
                return;          
            }
            m_enemy_ctrl.IsDead = true;

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
            Debug.Log("아이템 드랍");
            var item_prefab = m_enemy_ctrl.m_drop_item_manager.DropRandomItem(m_enemy_ctrl.m_drop_item_bag);

            if (item_prefab)
            {
                var drop_item = Instantiate(item_prefab);
                drop_item.transform.position = new Vector3(m_enemy_ctrl.gameObject.transform.position.x, m_enemy_ctrl.gameObject.transform.position.y + 3f, m_enemy_ctrl.gameObject.transform.position.z);
                drop_item.transform.SetParent(m_global_object.transform);
                Rigidbody item_rigid = drop_item.GetComponent<Rigidbody>();

                Vector3 dir = new Vector3(Random.Range(-3f, 3f), 7f, Random.Range(-3f, 3f));
                item_rigid.AddForce(dir, ForceMode.Impulse);
            }

            var money_prefab = m_enemy_ctrl.m_drop_item_manager.m_all_dorp_item_dic[ItemCode.MONEY].item_prefab;
            var drop_money_prefab = m_enemy_ctrl.m_drop_item_manager.DropRandomMoney(m_enemy_ctrl.m_drop_money_min, m_enemy_ctrl.m_drop_money_max, money_prefab);

            if (drop_money_prefab)
            {
                var drop_item = Instantiate(drop_money_prefab);
                drop_item.transform.position = new Vector3(m_enemy_ctrl.gameObject.transform.position.x, m_enemy_ctrl.gameObject.transform.position.y + 3f, m_enemy_ctrl.gameObject.transform.position.z);
                drop_item.transform.SetParent(m_global_object.transform);
                Rigidbody item_rigid = drop_item.GetComponent<Rigidbody>();

                Vector3 dir = new Vector3(Random.Range(-3f, 3f), 7f, Random.Range(-3f, 3f));
                item_rigid.AddForce(dir, ForceMode.Impulse);
            }
        }

        private void Destroy()
        {
            if (!m_enemy_ctrl.gameObject.activeInHierarchy) return;
            m_enemy_ctrl.IsDead = false;
            m_enemy_ctrl.ReturnToPool();
            
        }
    }
}