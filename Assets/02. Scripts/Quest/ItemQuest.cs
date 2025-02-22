using UnityEngine;

[System.Serializable]
public class ItemQuest : QuestBase
{
    [Header("획득해야 하는 아이템의 코드")]
    [SerializeField] private ItemCode m_target_item_code;
    public ItemCode ItemCode
    {
        get { return m_target_item_code; }
    }

    [Header("아이템을 획득해야 하는 횟수")]
    [SerializeField] private int m_total_count;
    public int TotalCount
    {
        get { return m_total_count; }
    }

    private int m_current_count;
    public int CurrentCount
    {
        get { return m_current_count; }
        set { m_current_count = value;}
    }

    public override string GetProgressText()
    {
        return $"{CurrentCount} / {TotalCount}";
    } 
}
