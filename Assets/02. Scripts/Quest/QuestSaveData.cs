using UnityEngine;
using UnityEngine.UIElements.Experimental;

[System.Serializable]
public class QuestSaveData
{
    [Header("퀘스트의 고유 ID")]
    [SerializeField] private int m_quest_id;
    public int ID
    {
        get { return m_quest_id; }
        set { m_quest_id = value; }
    }

    [Header("서브 킬 퀘스트의 목록")]
    [SerializeField] private KillQuest[] m_kill_quests;
    public KillQuest[] KillQuests
    {
        get { return m_kill_quests; }
        set { m_kill_quests = value; }
    }

    [Header("서브 아이템 퀘스트의 목록")]
    [SerializeField] private ItemQuest[] m_item_quests;
    public ItemQuest[] ItemQuests
    {
        get { return m_item_quests; }
        set { m_item_quests = value; }
    }

    [Header("현재 퀘스트 상태")]
    [SerializeField] private QuestState m_current_quest_state;
    public QuestState QuestState
    {
        get { return m_current_quest_state; }
        set { m_current_quest_state = value; }
    }
}

[System.Serializable]
public class QuestSaveDataList
{
    [SerializeField] QuestSaveData[] m_quest_save_datas;
    public QuestSaveData[] SaveDataList
    {
        get { return m_quest_save_datas; }
        set { m_quest_save_datas = value; }
    }
}