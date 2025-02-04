using UnityEngine;

namespace Junyoung
{
    [CreateAssetMenu(fileName = "EnemySpawnData", menuName = "Scriptable Objects/EnemySpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        [SerializeField]
        private Transform m_spawn_transform;
        public Transform SpawnTransform { get { return m_spawn_transform; } set { m_spawn_transform = value; } }

        [SerializeField]
        private EnemyType m_enemy_type;
        public EnemyType EnemyType { get { return m_enemy_type; } set { m_enemy_type= value; } }
    }
}