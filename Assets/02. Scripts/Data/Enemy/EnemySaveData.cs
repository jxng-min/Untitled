using UnityEngine;

namespace Junyoung
{
    [System.Serializable]
    public class EnemySaveData
    {
        public SVector3 m_position;
        public SQuaternion m_rotation;

        public EnemyStat m_stat;
        public EnemySpawnData m_spawn_data;
        public EnemyState m_enemy_state;

        public EnemySaveData(SVector3 position, SQuaternion rotation, EnemyStat stat, EnemySpawnData spawn_data, EnemyState enemy_state)
        {
            m_position = position;
            m_rotation = rotation;
            m_stat = stat;
            m_spawn_data = spawn_data;
            m_enemy_state = enemy_state;
        }
    }
}