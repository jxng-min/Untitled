using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;


namespace Junyoung
{
    public class EnemyBossCtrl : EnemyCtrl
    {
        private float m_detect_range = 15f;
        [Header("패널")]
        private GameObject m_canvas;
        [SerializeField] private GameObject m_hp_panel;
        [SerializeField] private Image m_hp_image;
        public bool IsPhaseTwo { get; set; }

        private bool m_is_phase_two_running = false;

        public bool IsNotCombating { get; set; } = false;

        public GameObject[] m_effect_prefabs;

        public List<GameObject> m_phase_two_effects;

        public new IObjectPool<EnemyBossCtrl> ManagedPool { get; set; }

        public override void Awake()
        {
            base.Awake();
            Destroy(gameObject.GetComponent<EnemyAttackState>());
            Destroy(gameObject.GetComponent<EnemyReadyState>());
            Destroy(gameObject.GetComponent<EnemyIdleState>());
            Destroy(gameObject.GetComponent<EnemyFollowState>());
            Destroy(gameObject.GetComponent<EnemyBackState>());
            m_enemy_attack_state = gameObject.AddComponent<EnemyBossAttackState>();
            m_enemy_ready_state = gameObject.AddComponent<EnemyBossReadyState>();
            m_enemy_idle_state = gameObject.AddComponent<EnemyBossIdleState>();
            m_enemy_follow_state = gameObject.AddComponent<EnemyBossFollowState>();
            m_enemy_back_state = gameObject.AddComponent<EnemyBossBackState>();

            m_canvas = GameObject.Find("Overlay Canvas");
            RectTransform[] UIs = m_canvas.transform.GetComponentsInChildren<RectTransform>(true);
            foreach (RectTransform UI in UIs)
            {
                if (UI.gameObject.name == "Boss HP Panel")
                {
                    m_hp_panel = UI.gameObject;
                }
                else if (UI.gameObject.name == "HP Bar")
                {
                    m_hp_image = UI.gameObject.GetComponent<Image>();
                }
            }
            ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem particle in particles)
            {
                m_phase_two_effects.Add(particle.gameObject);
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
            if (EnemyStat.HP <= OriginEnemyStat.HP / 2)
            {
                IsPhaseTwo = true;
            }
            else
            {
                IsPhaseTwo = false;
            }

            if (IsPhaseTwo)
            {
                if (!m_is_phase_two_running)
                {
                    m_is_phase_two_running = true;
                    StartCoroutine(Regeneration(1));
                    if (!m_phase_two_effects[0].activeSelf)
                    {
                        foreach (GameObject effect in m_phase_two_effects)
                        {
                            effect.SetActive(true);
                        }
                    }
                }

            }
            else
            {
                if (m_is_phase_two_running)
                {
                    m_is_phase_two_running = false;
                    foreach (GameObject effect in m_phase_two_effects)
                    {
                        effect.SetActive(false);
                    }
                }
            }
        }

        public IEnumerator Regeneration(float heal)
        {
            Debug.Log("ȸ������");
            while (!(StateContext.NowState is EnemyDeadState))
            {
                if (IsNotCombating)
                {
                    if (EnemyStat.HP + heal > OriginEnemyStat.HP)
                    {
                        EnemyStat.HP = OriginEnemyStat.HP;
                        break;
                    }
                    else
                    {
                        UpdateHP(heal);
                    }
                }
                else if (m_is_phase_two_running)
                {
                    if (EnemyStat.HP + heal > OriginEnemyStat.HP/2)
                    {
                        EnemyStat.HP = OriginEnemyStat.HP/2;                       
                    }
                    else
                    {
                        UpdateHP(heal);
                    }
                }
                yield return new WaitForSeconds(1f);
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
            Debug.Log("BossǮ �ʱ�ȭ");
            this.ManagedPool = pool;
        }
        public override void ReturnToPool()
        {
            Debug.Log($"{this.name} ��ȯ (Boss)");
            ManagedPool.Release(this as EnemyBossCtrl);
        }

        public void ActiveHpBar()
        {
            if (Vector3.Distance(EnemySpawnData.SpawnVector, Player.transform.position) <= EnemyStat.FollowRange && 
                !(StateContext.NowState is EnemyDeadState))
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
            if ((Vector3.Distance(EnemySpawnData.SpawnVector, Player.transform.position) <= m_detect_range)
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