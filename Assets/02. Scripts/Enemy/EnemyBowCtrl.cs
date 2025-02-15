using UnityEngine;
using UnityEngine.Pool;

namespace Junyoung
{

    public class EnemyBowCtrl : EnemyCtrl
    {
        public Transform m_arrow_spawn_pos;
        public new IObjectPool<EnemyBowCtrl> ManagedPool { get; set; }

        public override void Awake()
        {
            base.Awake();
            m_enemy_attack_state = gameObject.AddComponent<EnemyBowAttackState>();
            m_enemy_ready_state = gameObject.AddComponent<EnemyBowReadyState>();
        }
        public void SetEnemyPool(IObjectPool<EnemyBowCtrl> pool)
        {
            Debug.Log("Bow풀 초기화");
            this.ManagedPool = pool;
        }

        public override void SetDropItemBag()
        {
            ItemCode[] codes = new ItemCode[]
{
             ItemCode.SMALL_MP_POTION,
             ItemCode.BASIC_SHIELD,
             ItemCode.BASIC_ARMORPLATE
            };

            float[] chances = new float[]
            {
                37f,10f,5f
            };

            m_drop_item_manager.AddItemToBag(m_drop_item_bag, codes, chances);
        }

        public override void ReturnToPool()
        {
            Debug.Log($"{this.name} 반환 (Bow)");
            ManagedPool.Release(this as EnemyBowCtrl);  // 정확한 타입을 사용하여 풀로 반환
        }
    }
}