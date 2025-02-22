using UnityEngine;

[System.Serializable]
public struct QuestContentData
{
    public int m_quest_id;
    [TextArea] public string m_title;
    [TextArea] public string m_receive_from;
    [TextArea] public string m_compact_content;
    [TextArea] public string m_full_content;
}

[System.Serializable]
public class QuestContentDataList
{
    [SerializeField] private QuestContentData[] m_data_list;
    public QuestContentData[] DataList
    {
        get { return m_data_list; }
    } 
}