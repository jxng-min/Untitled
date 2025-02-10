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
            int item_index = Random.Range(0, m_enemy_ctrl.m_drop_item_prefabs.Length);
            var drop_item = Instantiate(m_enemy_ctrl.m_drop_item_prefabs[item_index]);
            drop_item.transform.position = new Vector3(m_enemy_ctrl.gameObject.transform.position.x, m_enemy_ctrl.gameObject.transform.position.y + 3f, m_enemy_ctrl.gameObject.transform.position.z);
            Rigidbody item_rigid = drop_item.GetComponent<Rigidbody>();

            Vector3 dir = new Vector3(Random.Range(-3f, 3f), 7f, Random.Range(-3f, 3f));
            item_rigid.AddForce(dir, ForceMode.Impulse);

        }

        private void Destroy()
        {
            m_enemy_ctrl.ReturnToPool();
        }
    }
}