using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junyoung
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private EnemyFactory m_enemy_factory;

        public int[] m_max_enemy_size = { 15, 10, 1 };

        [Header("�� ��ȯ ��ġ")]
        public Transform[] m_spawn_transforms;
        public Transform m_boss_spawn_transform;

        public Dictionary<Transform, Dictionary<EnemyType, int>> m_active_enemy_counts = new Dictionary<Transform, Dictionary<EnemyType, int>>();

        private Dictionary<EnemyType, int> m_max_enemy_by_type = new Dictionary<EnemyType, int> { { EnemyType.Axe, 2 },{ EnemyType.Bow ,1},{ EnemyType.Boss, 0 } };

        Coroutine m_spawn_coroutine;
        Coroutine m_boss_coroutine;
        void Start()
        {
            m_enemy_factory = GetComponent<EnemyFactory>();

        }

        IEnumerator SpawnMangement() // ���� ��ȯ�Ǿ��ִ� ��
        {
            while (true)
            {
                if (GameManager.Instance.GameStatus == GameEventType.PLAYING || GameManager.Instance.GameStatus == GameEventType.DEAD)
                {
                    yield return new WaitForSeconds(15f);
                    foreach (var spawn_pos in m_spawn_transforms)
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
                if (GameManager.Instance.GameStatus == GameEventType.PLAYING || GameManager.Instance.GameStatus == GameEventType.DEAD)
                {
                    if (!m_active_enemy_counts.ContainsKey(m_boss_spawn_transform))
                    {
                        m_active_enemy_counts[m_boss_spawn_transform] = new Dictionary<EnemyType, int>();

                        m_active_enemy_counts[m_boss_spawn_transform][EnemyType.Boss] = 0;

                    }
                    yield return new WaitForSeconds(10f);
                    if (m_active_enemy_counts[m_boss_spawn_transform][EnemyType.Boss] < 1)
                    {
                        m_enemy_factory.SpawnEnemy(EnemyType.Boss, m_boss_spawn_transform);
                    }
                }
                yield return null;
            }
        }

        private void Update()
        {
            if( (GameManager.Instance.GameStatus == GameEventType.PLAYING 
                || GameManager.Instance.GameStatus == GameEventType.DEAD))
            {
                if (m_spawn_coroutine == null)
                {
                    Debug.Log($"{GameManager.Instance.GameStatus.ToString()}");
                    Debug.Log("�� ��ȯ �ڷ�ƾ ����");
                    m_spawn_coroutine = StartCoroutine(SpawnMangement());
                    m_boss_coroutine = StartCoroutine(SpawnBossMangement());
                }
            }
            else 
            {
                if (m_spawn_coroutine != null)
                {
                    Debug.Log($"{GameManager.Instance.GameStatus.ToString()}");
                    Debug.Log("�� ��ȯ �ڷ�ƾ �Ͻ� ����");
                    StopCoroutine(m_spawn_coroutine);
                    StopCoroutine(m_boss_coroutine);

                    m_spawn_coroutine = null;
                    m_boss_coroutine= null;
                }
            }
        }
    }
}