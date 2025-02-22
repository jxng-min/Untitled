using UnityEngine;
using Junyoung;

[System.Serializable]
public class KillQuest : QuestBase
{
    [Header("처치해야 하는 적 몬스터의 코드")]
    [SerializeField] private EnemyType m_target_enemy_code;
    public EnemyType EnemyType
    {
        get { return m_target_enemy_code; }
    }

    [Header("몬스터를 처치해야 하는 횟수")]
    [SerializeField] private int m_total_count;
    public int TotalCount
    {
        get { return m_total_count; }
    }

    [Header("현재 몬스터를 처치한 횟수")]
    [SerializeField] private int m_current_count;
    public int CurrentCount
    {
        get { return m_current_count; }
        set { m_current_count = value; }
    }

    public override string GetProgressText()
    {
        return $"{CurrentCount} / {TotalCount}";
    }
}