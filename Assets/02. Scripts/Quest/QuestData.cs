using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable Object/Create Quest")]
public class QuestData : ScriptableObject
{
    public QuestState QuestState = QuestState.NEVER_RECEIVED;

    [Header("퀘스트의 고유한 ID")]
    [SerializeField] private int m_quest_id;
    public int ID
    {
        get { return m_quest_id; }
    }

    [Header("퀘스트 목표")]
    [SerializeField] private KillQuest[] m_kill_quests;
    public KillQuest[] KillQuests
    {
        get { return m_kill_quests; }
    }

    [SerializeField] public ItemQuest[] m_item_quests;
    public ItemQuest[] ItemQuests
    {
        get { return m_item_quests; }
    }


    [Header("퀘스트 수락을 위한 선행 퀘스트 ID의 목록")]
    [SerializeField] private int[] m_prerequisite_quest_ids;
    public int[] PrerequisteQuestIDs
    {
        get { return m_prerequisite_quest_ids; }
    }

    [Header("퀘스트 수락을 위한 최소 레벨")]
    [SerializeField] private int m_prerequisite_player_level;
    public int PrerequisteLevel
    {
        get { return m_prerequisite_player_level; }
    }

    [Header("퀘스트 완료 보상으로 받을 경험치")]
    [SerializeField] private int m_reward_exp;
    public int EXP
    {
        get { return m_reward_exp; }
    }

    [Header("퀘스트 완료 보상으로 받을 아이템 목록")]
    [SerializeField] private Item[] m_reward_items;
    public Item[] Items
    {
        get { return m_reward_items; }
    }

    [Header("보상 아이템별 개수 목록")]
    [SerializeField] private int[] m_reward_items_counts;
    public int[] ItemCounts
    {
        get { return m_reward_items_counts; }
    }

    [Header("퀘스트를 진행하는 씬의 이름")]
    [SerializeField] private string m_quest_scene_name;
    public string Scene
    {
        get { return m_quest_scene_name; }
    }

    [Header("퀘스트 의뢰인의 위치")]
    [SerializeField] private Vector3 m_source_position;
    public Vector3 Source
    {
        get { return m_source_position; }
    }

    [Header("퀘스트를 수행할 목적지의 위치")]
    [SerializeField] private Vector3 m_destination_position;
    public Vector3 Destination
    {
        get { return m_destination_position; }
    }

    [Header("해당 퀘스트가 가지는 모든 부분 퀘스트")]
    [SerializeField] private QuestBase[] m_all_quests;
    public QuestBase[] All
    {
        get { return m_all_quests; }
        set { m_all_quests = value; }
    }
}
