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

        private Dictionary<EnemyType, IObjectPool<EnemyCtrl>> m_enemy_pools;

        private EnemySpawnManager m_enemy_spawn_manager;

        private EnemyType m_now_type;
        private Transform m_now_transform;
        
        private void Awake()
        {
            m_enemy_spawn_manager = GetComponent<EnemySpawnManager>();
            m_enemy_pools = new Dictionary<EnemyType, IObjectPool<EnemyCtrl>>();

            for(int i =0; i< m_enemy_prefab.Count; i++)
            {
                m_enemy_pools[(EnemyType)i] = new ObjectPool<EnemyCtrl>(() => CreateEnemy((EnemyType)i), OnGetEnemy, OnReturnEnemy,
                                                                                OnDestoryEnemy, maxSize: m_enemy_spawn_manager.m_max_enemy_size[i]);
                Debug.Log($"{(EnemyType)i} 타입 ObjectPool 초기화");
            }
        }

        public void SpawnEnemy(EnemyType type, Transform spawn_pos)
        {
            var new_enemy = m_enemy_pools[type].Get();
            new_enemy.OriginEnemyStat = m_enemy_stat_list[(int)type];
            new_enemy.InitStat();
            new_enemy.transform.position = spawn_pos.position ;
            new_enemy.EnemySpawnData.SpawnTransform= spawn_pos ;
            new_enemy.EnemySpawnData.EnemyType = type ;
            new_enemy.PatrolCenter = spawn_pos;
            m_enemy_spawn_manager.m_active_enemy_counts[spawn_pos][type]++;
        }

        private EnemyCtrl CreateEnemy(EnemyType type)
        {
            var newEnemy = Instantiate(m_enemy_prefab[(int)type]).GetComponent<EnemyCtrl>();
            newEnemy.SetEnemyPool(m_enemy_pools[type]);
            return newEnemy;
        }

        private void OnGetEnemy(EnemyCtrl enemy)
        {
            enemy.gameObject.SetActive(true);
            m_enemy_spawn_manager.m_active_enemy_counts[enemy.EnemySpawnData.SpawnTransform][enemy.EnemySpawnData.EnemyType]--;
        }

        public void OnReturnEnemy(EnemyCtrl enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void OnDestoryEnemy(EnemyCtrl enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}