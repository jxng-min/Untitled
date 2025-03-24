using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junyoung
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private EnemyFactory m_enemy_factory;
        private EnemySaveLoadManager m_save_load_manager;

        public int[] m_max_enemy_size = { 15, 10, 1 };


        [Header("적 소환 위치")]
        public Vector3[] m_spawn_vectors;
        public Vector3 m_boss_spawn_vector;

        public Dictionary<Vector3, Dictionary<EnemyType, int>> m_active_enemy_counts = new Dictionary<Vector3, Dictionary<EnemyType, int>>();

        private Dictionary<EnemyType, int> m_max_enemy_by_type = new Dictionary<EnemyType, int> { { EnemyType.Axe, 2 },{ EnemyType.Bow ,1},{ EnemyType.Boss, 0 } };

        Coroutine m_spawn_coroutine;
        Coroutine m_boss_coroutine;

        private void Awake()
        {
            //m_active_enemy_counts 딕셔너리 초기화
            foreach (var spawn_pos in m_spawn_vectors)
            {
                if (!m_active_enemy_counts.ContainsKey(spawn_pos))
                {
                    m_active_enemy_counts[spawn_pos] = new Dictionary<EnemyType, int>();

                    foreach (EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
                    {
                        m_active_enemy_counts[spawn_pos][type] = 0;
                    }
                }
            }
            if (!m_active_enemy_counts.ContainsKey(m_boss_spawn_vector))
            {
                m_active_enemy_counts[m_boss_spawn_vector] = new Dictionary<EnemyType, int>();

                m_active_enemy_counts[m_boss_spawn_vector][EnemyType.Boss] = 0;
            }
        }

        void Start()
        {
            m_enemy_factory = GetComponent<EnemyFactory>();
            m_save_load_manager = GameObject.Find("Eenemy Save Load Manager").GetComponent<EnemySaveLoadManager>();

            LoadEnemies();
        }


        IEnumerator SpawnMangement() // 몬스터 수를 체크후 소환하는 코루틴
        {
            while (true) // 체크
            {
                if (GameManager.Instance.GameState == GameEventType.PLAYING || GameManager.Instance.GameState == GameEventType.DEAD)
                {                 
                    foreach (var spawn_pos in m_spawn_vectors)
                    {
                        yield return new WaitForSeconds(15f);
                        foreach (EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
                        {
                            if (m_active_enemy_counts[spawn_pos][type] < m_max_enemy_by_type[type])
                            {
                                m_enemy_factory.SpawnEnemy(type, spawn_pos);
                                yield return new WaitForSeconds(15f);
                            }
                        }
                    }
                }
                yield return null;
            }
        }
        IEnumerator SpawnBossMangement()
        {
            while (true)
            {
                if (GameManager.Instance.GameState == GameEventType.PLAYING || GameManager.Instance.GameState == GameEventType.DEAD)
                {
                    yield return new WaitForSeconds(10f);
                    if (m_active_enemy_counts[m_boss_spawn_vector][EnemyType.Boss] < 1)
                    {
                        m_enemy_factory.SpawnEnemy(EnemyType.Boss, m_boss_spawn_vector);
                    }
                }
                yield return null;
            }
        }

        public void LoadEnemies()
        {
            List<EnemySaveData> data_list = m_save_load_manager.LoadEnemies();
            if (data_list == null) return;

            foreach(EnemySaveData data in data_list)
            {
                m_enemy_factory.SpawnEnemy(data.m_position, data.m_rotation, data.m_stat.ToEnemyStat(), data.m_spawn_data.ToEnemySpawnData(), data.m_enemy_state);
            }
        }

        private void Update()
        {
            if( (GameManager.Instance.GameState == GameEventType.PLAYING 
                || GameManager.Instance.GameState == GameEventType.DEAD))
            {
                if (m_spawn_coroutine == null)
                {

                    Debug.Log("적 소환 코루틴 시작");
                    m_spawn_coroutine = StartCoroutine(SpawnMangement());
                    m_boss_coroutine = StartCoroutine(SpawnBossMangement());
                }
            }
            else 
            {
                if (m_spawn_coroutine != null)
                {
                    Debug.Log("적 소환 코루틴 일시 정지");
                    StopCoroutine(m_spawn_coroutine);
                    StopCoroutine(m_boss_coroutine);

                    m_spawn_coroutine = null;
                    m_boss_coroutine= null;
                }
            }
        }
    }
}