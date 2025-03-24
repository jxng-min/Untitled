using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Junyoung
{



    public class EnemyFactory : MonoBehaviour
    {
        [Header("생성되는 적 Stat List")]
        [SerializeField]
        private List<EnemyStat> m_enemy_stat_list;
        [SerializeField]
        private List<GameObject> m_enemy_prefab;
        [SerializeField]
        private Dictionary<EnemyType, int> m_enemy_max_pool;

        private ObjectPool<EnemyCtrl> m_axe_pools;
        private  ObjectPool<EnemyBowCtrl> m_bow_pools;
        private ObjectPool<EnemyBossCtrl> m_boss_pools;

        private EnemySpawnManager m_enemy_spawn_manager;

        private GameObject m_global_object;


        private void Awake()
        {
            m_enemy_spawn_manager = GetComponent<EnemySpawnManager>();

            m_global_object = GameObject.Find("[Global]");

            for (int i =0; i< m_enemy_prefab.Count; i++)
            {
                EnemyType type = (EnemyType)i;
                switch (type)
                {
                    case EnemyType.Bow:
                        m_bow_pools = new ObjectPool<EnemyBowCtrl>(
                            () => CreateEnemy<EnemyBowCtrl>(type),
                            OnGetEnemy, OnReturnEnemy, OnDestoryEnemy,
                            maxSize: m_enemy_spawn_manager.m_max_enemy_size[i]
                        );
                        break;
                    case EnemyType.Boss:
                        m_boss_pools = new ObjectPool<EnemyBossCtrl>(
                            () => CreateEnemy<EnemyBossCtrl>(type),
                            OnGetEnemy, OnReturnEnemy, OnDestoryEnemy,
                            maxSize: m_enemy_spawn_manager.m_max_enemy_size[i]
                        );
                        break;
                    case EnemyType.Axe:
                    default:
                        m_axe_pools = new ObjectPool<EnemyCtrl>(
                            () => CreateEnemy<EnemyCtrl>(type),
                            OnGetEnemy, OnReturnEnemy, OnDestoryEnemy,
                            maxSize: m_enemy_spawn_manager.m_max_enemy_size[i]
                        );
                        break;
                }
            }
        }

        public void SpawnEnemy(EnemyType type, Vector3 spawn_pos)
        {
            EnemyCtrl new_enemy = null;
            switch (type)
            {
                case EnemyType.Bow:
                    {
                        new_enemy = m_bow_pools.Get();
                        (new_enemy as EnemyBowCtrl).SetEnemyPool(m_bow_pools); 
                       
                    }
                    break;
                case EnemyType.Boss:
                    {
                        new_enemy = m_boss_pools.Get();
                        (new_enemy as EnemyBossCtrl).SetEnemyPool(m_boss_pools);

                    }
                    break;
                case EnemyType.Axe:
                default:
                    new_enemy = m_axe_pools.Get();
                    new_enemy.SetEnemyPool(m_axe_pools);
                    break;
            }

            new_enemy.OriginEnemyStat = m_enemy_stat_list[(int)type];
            new_enemy.transform.SetParent(m_global_object.transform);
            new_enemy.Agent.Warp(spawn_pos);
            new_enemy.EnemySpawnData.SpawnVector = spawn_pos;
            new_enemy.EnemySpawnData.EnemyType = type ;

            m_enemy_spawn_manager.m_active_enemy_counts[spawn_pos][type]++;
        }

        public void SpawnEnemy(SVector3 vector,SQuaternion quaternion, EnemyStat stat, EnemySpawnData spawn_data, EnemyState state)
        {
            EnemyCtrl new_enemy = null;
            switch (spawn_data.EnemyType)
            {
                case EnemyType.Bow:
                    {
                        new_enemy = m_bow_pools.Get();
                        (new_enemy as EnemyBowCtrl).SetEnemyPool(m_bow_pools);
                    }
                    break;
                case EnemyType.Boss:
                    {
                        new_enemy = m_boss_pools.Get();
                        (new_enemy as EnemyBossCtrl).SetEnemyPool(m_boss_pools);
                    }
                    break;
                case EnemyType.Axe:
                default:
                    new_enemy = m_axe_pools.Get();
                    new_enemy.SetEnemyPool(m_axe_pools);
                    break;
            }
            new_enemy.LoadInit(vector, quaternion, m_enemy_stat_list[(int)spawn_data.EnemyType],
                stat, spawn_data, state, m_global_object);
            Debug.Log($"소환 백터 Vector3 : {spawn_data.SpawnVector} ");
            m_enemy_spawn_manager.m_active_enemy_counts[spawn_data.SpawnVector][spawn_data.EnemyType]++;
        }

        private T CreateEnemy<T>(EnemyType type) where T : EnemyCtrl
        {
            return Instantiate(m_enemy_prefab[(int)type]).GetComponent<T>();
        }

        private void OnGetEnemy(EnemyCtrl enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        public void OnReturnEnemy(EnemyCtrl enemy)
        {
            enemy.gameObject.SetActive(false);
            m_enemy_spawn_manager.m_active_enemy_counts[enemy.EnemySpawnData.SpawnVector][enemy.EnemySpawnData.EnemyType]--;
        }

        private void OnDestoryEnemy(EnemyCtrl enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}