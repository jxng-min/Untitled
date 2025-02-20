using UnityEngine;

[System.Serializable]
public class ChestInfo
{
    [SerializeField] private int m_chest_id;
    public int ID
    {
        get { return m_chest_id; }
    } 

    [SerializeField] private ChestSlotInfo[] m_slot_infos;
    public ChestSlotInfo[] SlotInfos
    {
        get { return m_slot_infos; }
    }

    public ChestInfo(int chest_id, ChestSlotInfo[] slots)
    {
        m_chest_id = chest_id;
        m_slot_infos = slots;
    }
}

[System.Serializable]
public class ChestInfoList
{
    public ChestInfo[] m_chest_infos;

    public ChestInfoList(ChestInfo[] chest_infos)
    {
        m_chest_infos = chest_infos;
    }
}