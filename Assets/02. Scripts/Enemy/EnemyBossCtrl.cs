using UnityEngine;
using UnityEngine.Pool;
using System.Collections;


namespace Junyoung
{
    public class EnemyBossCtrl : EnemyCtrl
    {
        public float m_detect_range = 15f;
        public float m_hp_bar_range;

        public GameObject[] m_effect_prefabs;

        public new IObjectPool<EnemyBossCtrl> ManagedPool { get; set; }


        public override void Awake()
        {
            base.Awake();
            Destroy(gameObject.GetComponent<EnemyAttackState>());
            Destroy(gameObject.GetComponent<EnemyReadyState>());
            Destroy(gameObject.GetComponent<EnemyIdleState>());
            m_enemy_attack_state = gameObject.AddComponent<EnemyBossAttackState>();
            m_enemy_ready_state = gameObject.AddComponent<EnemyBossReadyState>();
            m_enemy_idle_state = gameObject.AddComponent<EnemyBossIdleState>();
        }
        public override void SetDropItemBag()
        {
            ItemCode[] codes = new ItemCode[]
{
             ItemCode.BASIC_SHIELD,
             ItemCode.BASIC_ARMORPLATE,
             ItemCode.BASIC_HELMET,
             ItemCode.BASIC_SHOES
            };

            float[] chances = new float[]
            {
                25f,25f,25f,25f
            };

            m_drop_item_manager.AddItemToBag(m_drop_item_bag, codes, chances);
        }
        public void SetEnemyPool(IObjectPool<EnemyBossCtrl> pool)
        {
            Debug.Log("Boss풀 초기화");
            this.ManagedPool = pool;
        }
        public override void ReturnToPool()
        {
            Debug.Log($"{this.name} 반환 (Boss)");
            ManagedPool.Release(this as EnemyBossCtrl);
        }
        public override void DetectPlayer()
        {
            if ((Vector3.Distance(EnemySpawnData.SpawnTransform.position, Player.transform.position) <= m_detect_range)
                && !(Player.GetComponent<PlayerCtrl>().StateContext.Current is PlayerDeadState))
            {
                ChangeState(EnemyState.FOUNDPLAYER);
            }
        }

        public void Effect(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        var effect = Instantiate(m_effect_prefabs[index]);
                        effect.transform.position= new Vector3(transform.position.x,0,transform.position.z);
                        StartCoroutine(DestroyEffect(effect, 2));
                        break;
                    }
            }
        }

        private IEnumerator DestroyEffect(GameObject effect,float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(effect);
        }
    }
}