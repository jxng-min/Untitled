using System.Collections.Generic;
using UnityEngine;

namespace Junyoung
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private EnemyFactory m_enemy_factory;

        public int[] m_max_enemy_size = { 15, 10, 1 };

        [Header("적 소환 위치")]
        public Transform[] m_spawn_transforms;

        public Dictionary<Transform, Dictionary<EnemyType, int>> m_active_enemy_counts = new Dictionary<Transform, Dictionary<EnemyType, int>>();

        private Dictionary<EnemyType, int> m_max_enemy_by_type = new Dictionary<EnemyType, int> { { EnemyType.Axe, 3 },{ EnemyType.Bow ,0} ,{ EnemyType.Boss ,0} };
        
        void Start()
        {
            m_enemy_factory = GetComponent<EnemyFactory>();
        }

        void Update()
        {
            foreach ( var spawn_pos in m_spawn_transforms)
            {
                if(!m_active_enemy_counts.ContainsKey(spawn_pos))
                {
                    m_active_enemy_counts[spawn_pos] = new Dictionary<EnemyType, int> {
                        {EnemyType.Axe,0 } };
                }              
                foreach(EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
                {
                    if (m_active_enemy_counts[spawn_pos][type] < m_max_enemy_by_type[type])
                    {
                        m_enemy_factory.SpawnEnemy(type, spawn_pos);
                    }
                }


            }
        }
    }
}