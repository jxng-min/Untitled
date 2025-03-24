using Junyoung;
using UnityEngine;

[System.Serializable]
public class SEnemySpawnData 
{
    public SVector3 m_spawn_vector;
    public EnemyType m_enemy_type;

    public SEnemySpawnData(EnemySpawnData spawn_data)
    {
        m_spawn_vector = new SVector3( spawn_data.SpawnVector);
        m_enemy_type = spawn_data.EnemyType;
    }

    public EnemySpawnData ToEnemySpawnData()
    {
        EnemySpawnData data = new EnemySpawnData();
        data.SpawnVector = m_spawn_vector.ToVector3();
        data.EnemyType = m_enemy_type;

        return data;
    }
}
