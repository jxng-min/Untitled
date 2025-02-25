using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System.Collections;


namespace Junyoung
{
    public class EnemyBossCtrl : EnemyCtrl
    {
        private float m_detect_range = 15f;
        [Header("보스 체력바")]
        private GameObject m_canvas;
        [SerializeField] private GameObject m_hp_panel;
        [SerializeField] private Image m_hp_image;
        public bool IsPhaseTwo { get; set; }

        private bool m_is_regenerationing = false;

        public GameObject[] m_effect_prefabs;

        public new IObjectPool<EnemyBossCtrl> ManagedPool { get; set; }

        public override void Awake()
        {
            base.Awake();
            Destroy(gameObject.GetComponent<EnemyAttackState>());
            Destroy(gameObject.GetComponent<EnemyReadyState>());
            Destroy(gameObject.GetComponent<EnemyIdleState>());
            Destroy(gameObject.GetComponent<EnemyFollowState>());
            m_enemy_attack_state = gameObject.AddComponent<EnemyBossAttackState>();
            m_enemy_ready_state = gameObject.AddComponent<EnemyBossReadyState>();
            m_enemy_idle_state = gameObject.AddComponent<EnemyBossIdleState>();
            m_enemy_follow_state = gameObject.AddComponent<EnemyBossFollowState>();

            m_canvas = GameObject.Find("Canvas");
            RectTransform[] UIs =  m_canvas.transform.GetComponentsInChildren<RectTransform>(true);
            foreach(RectTransform UI in UIs)
            {
                if(UI.gameObject.name == "Boss HP Panel")
                {
                    m_hp_panel = UI.gameObject;
                }
                else if(UI.gameObject.name == "HP Bar")
                {
                    m_hp_image = UI.gameObject.GetComponent<Image>();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (m_hp_panel)
            { 
                m_hp_image.fillAmount = EnemyStat.HP / OriginEnemyStat.HP;
                ActiveHpBar();
            }
            if(EnemyStat.HP <= OriginEnemyStat.HP/2)
            {
                IsPhaseTwo = true;
            }
            else
            {
                IsPhaseTwo = false;
            }

            if(IsPhaseTwo)
            {
                if (!m_is_regenerationing)
                {
                    StartCoroutine(Regeneration());
                }
            }
            else
            {
                m_is_regenerationing = false;
            }
        }

        private IEnumerator Regeneration()
        {
            m_is_regenerationing= true;
            while(!(StateContext.NowState is EnemyDeadState))
            {
                if (!m_is_regenerationing)
                {
                    break;
                }
                yield return new WaitForSeconds(1f);
                UpdateHP(1f);
            }
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

        public void ActiveHpBar()
        {
            if (Vector3.Distance(EnemySpawnData.SpawnTransform.position, Player.transform.position) <= EnemyStat.FollowRange)
            {
                m_hp_panel.SetActive(true);
            }
            else
            {
                m_hp_panel.SetActive(false);
            }
        }

        public override void DetectPlayer()
        {
            if ((Vector3.Distance(EnemySpawnData.SpawnTransform.position, Player.transform.position) <= m_detect_range)
                && !(Player.GetComponent<PlayerCtrl>().StateContext.Current is PlayerDeadState))
            {
                ChangeState(EnemyState.FOUNDPLAYER);
            }
        }

        public GameObject Effect(int index, Vector3 pos)
        {
            var effect = Instantiate(m_effect_prefabs[index], pos, m_effect_prefabs[index].transform.rotation);

            return effect;
        }

        public IEnumerator DestroyEffect(GameObject effect,float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(effect);
        }
    }
}