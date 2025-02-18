using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC 정보")]
    [SerializeField] private NPC m_npc_info;
    public NPC Info
    {
        get { return m_npc_info; }
    }

    [Header("NPC 인디케이터의 높이")]
    [SerializeField] private float m_indicator_height;
    public float Indicator
    {
        get { return m_indicator_height; }
    }
}