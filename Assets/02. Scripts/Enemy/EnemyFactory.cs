using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Junyoung
{



    public class EnemyFactory : MonoBehaviour
    {
        [Header("�����Ǵ� �� Stat List")]
        [SerializeField]
        private List<EnemyStat> m_enemy_stat_list;
        [SerializeField]
        private List<GameObject> m_enemy_prefab;
        [SerializeField]
        private Dictionary<EnemyType, int> m_enemy_max_pool;

        private Dictionary<EnemyType, IObjectPool<EnemyCtrl>> m_enemy_pools;

        private EnemySpawnManager m_enemy_spawn_manager;
        
        private void Awake()
        {
            Debug.Log($"m_enemy_stat_list.Count: {m_enemy_stat_list.Count}");
            Debug.Log($"m_enemy_prefab.Count: {m_enemy_prefab.Count}");
            m_enemy_spawn_manager = GetComponent<EnemySpawnManager>();
            m_enemy_pools = new Dictionary<EnemyType, IObjectPool<EnemyCtrl>>();

            for(int i =0; i< m_enemy_prefab.Count; i++)
            {
                EnemyType type = (EnemyType)i;
                Debug.Log($"[�����] ObjectPool �ʱ�ȭ, Ÿ��: {type} (�ε���: {i})");
                m_enemy_pools[type] = new ObjectPool<EnemyCtrl>(() => CreateEnemy(type), OnGetEnemy, OnReturnEnemy,
                                                                                OnDestoryEnemy, maxSize: m_enemy_spawn_manager.m_max_enemy_size[i]);
                Debug.Log($"{type} Ÿ�� ObjectPool �ʱ�ȭ");
            }
        }

        public void SpawnEnemy(EnemyType type, Transform spawn_pos)
        {
            var new_enemy = m_enemy_pools[type].Get();

            new_enemy.OriginEnemyStat = m_enemy_stat_list[(int)type];
            new_enemy.InitComponent();
            new_enemy.InitStat();
            new_enemy.Agent.Warp(spawn_pos.position);
            new_enemy.EnemySpawnData.SpawnTransform = spawn_pos;
            new_enemy.EnemySpawnData.EnemyType = type ;
            
            m_enemy_spawn_manager.m_active_enemy_counts[spawn_pos][type]++;
            Debug.Log($"��ȯ ��ġ : {spawn_pos} Ÿ�� : {type} ���� ��ȯ �� : {m_enemy_spawn_manager.m_active_enemy_counts[spawn_pos][type]}");
        } 

        private EnemyCtrl CreateEnemy(EnemyType type)
        {
            Debug.Log($"CreateEnemy Ÿ�� : {type} (�ε���: {(int)type})");
            if ((int)type < 0 || (int)type >= m_enemy_prefab.Count)
            {
                Debug.LogError($"[Error] m_enemy_prefab�� {type}�� �������� ����! (�ε���: {(int)type})");
                return null;
            }
            var new_enemy = Instantiate(m_enemy_prefab[(int)type]).GetComponent<EnemyCtrl>();
            new_enemy.SetEnemyPool(m_enemy_pools[type]);
            return new_enemy;
        }

        private void OnGetEnemy(EnemyCtrl enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        public void OnReturnEnemy(EnemyCtrl enemy)
        {
            enemy.gameObject.SetActive(false);
            m_enemy_spawn_manager.m_active_enemy_counts[enemy.EnemySpawnData.SpawnTransform][enemy.EnemySpawnData.EnemyType]--;
            Debug.Log($"��ȯ ��ġ : {enemy.EnemySpawnData.SpawnTransform} Ÿ�� : {enemy.EnemySpawnData.EnemyType} ���� ��ȯ �� : {m_enemy_spawn_manager.m_active_enemy_counts[enemy.EnemySpawnData.SpawnTransform][enemy.EnemySpawnData.EnemyType]}");
        }

        private void OnDestoryEnemy(EnemyCtrl enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}