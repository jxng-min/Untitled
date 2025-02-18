using UnityEngine;
using UnityEngine.Pool;

namespace Junyoung
{
    public class EnemyBossCtrl : EnemyCtrl
    {
        public float m_detect_range = 15f;
        public float m_hp_bar_range;

        void Start()
        {

        }

        void Update()
        {

        }

        public override void Awake()
        {
            base.Awake();
            m_enemy_attack_state = gameObject.AddComponent<EnemyBowAttackState>();
            m_enemy_ready_state = gameObject.AddComponent<EnemyBowReadyState>();
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

        public override void DetectPlayer()
        {
            if (Vector3.Distance(EnemySpawnData.SpawnTransform.position, transform.position) <= m_detect_range)
            {
                ChangeState(EnemyState.FOUNDPLAYER);
            }
        }
    }
}