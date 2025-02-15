using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Scriptable Object/Create NPC")]
public class NPC : ScriptableObject
{
    [Header("NPC의 고유한 NPC ID")]
    [SerializeField] private int m_npc_id;
    public int ID
    {
        get { return m_npc_id; }
    }

    [Header("NPC의 직업")]
    [SerializeField] private NPCType m_npc_type;
    public NPCType Type
    {
        get { return m_npc_type; }
    }

    [Header("NPC의 상호작용 여부")]
    [SerializeField] private bool m_can_interaction;
    public bool Interaction
    {
        get { return m_can_interaction; }
    }

    [Header("현재 퀘스트의 진행 여부")]
    [SerializeField] private bool m_is_quest;
    public bool IsQuest
    {
        get { return m_is_quest; }
    }
}
