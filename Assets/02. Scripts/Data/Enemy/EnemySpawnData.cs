using UnityEngine;

namespace Junyoung
{
    [CreateAssetMenu(fileName = "EnemySpawnData", menuName = "Scriptable Objects/EnemySpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        [SerializeField]
        private SVector3 m_spawn_vector;
        public SVector3 SpawnVector { get { return m_spawn_vector; } set { m_spawn_vector = value; } }

        [SerializeField]
        private EnemyType m_enemy_type;
        public EnemyType EnemyType { get { return m_enemy_type; } set { m_enemy_type= value; } }
    }
}