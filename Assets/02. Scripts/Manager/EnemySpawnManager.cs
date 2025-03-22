using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junyoung
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private EnemyFactory m_enemy_factory;

        public int[] m_max_enemy_size = { 15, 10, 1 };


        [Header("적 소환 위치")]
        public SVector3[] m_spawn_vectors;
        public SVector3 m_boss_spawn_vector;

        public Dictionary<SVector3, Dictionary<EnemyType, int>> m_active_enemy_counts = new Dictionary<SVector3, Dictionary<EnemyType, int>>();

        private Dictionary<EnemyType, int> m_max_enemy_by_type = new Dictionary<EnemyType, int> { { EnemyType.Axe, 2 },{ EnemyType.Bow ,1},{ EnemyType.Boss, 0 } };

        Coroutine m_spawn_coroutine;
        Coroutine m_boss_coroutine;
        void Start()
        {
            m_enemy_factory = GetComponent<EnemyFactory>();

        }


        IEnumerator SpawnMangement() // 현재 소환되어있는 몬
        {
            while (true)
            {
                if (GameManager.Instance.GameState == GameEventType.PLAYING || GameManager.Instance.GameState == GameEventType.DEAD)
                {
                    yield return new WaitForSeconds(15f);
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
                    if (!m_active_enemy_counts.ContainsKey(m_boss_spawn_vector))
                    {
                        m_active_enemy_counts[m_boss_spawn_vector] = new Dictionary<EnemyType, int>();

                        m_active_enemy_counts[m_boss_spawn_vector][EnemyType.Boss] = 0;

                    }
                    yield return new WaitForSeconds(10f);
                    if (m_active_enemy_counts[m_boss_spawn_vector][EnemyType.Boss] < 1)
                    {
                        m_enemy_factory.SpawnEnemy(EnemyType.Boss, m_boss_spawn_vector);
                    }
                }
                yield return null;
            }
        }

        private void Update()
        {
            if( (GameManager.Instance.GameState == GameEventType.PLAYING 
                || GameManager.Instance.GameState == GameEventType.DEAD))
            {
                if (m_spawn_coroutine == null)
                {

                    Debug.Log($"{GameManager.Instance.GameStatus.ToString()}");
                    Debug.Log("적 소환 코루틴 시작");
                    m_spawn_coroutine = StartCoroutine(SpawnMangement());
                    m_boss_coroutine = StartCoroutine(SpawnBossMangement());
                }
            }
            else 
            {
                if (m_spawn_coroutine != null)
                {
                    Debug.Log($"{GameManager.Instance.GameStatus.ToString()}");
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