using UnityEngine;

[System.Serializable]
public class ChestSlotInfo
{
    [SerializeField] private int m_item_id;
    public int ID
    {
        get { return m_item_id; }
    }

    [SerializeField] private int m_item_count;
    public int Count
    {
        get { return m_item_count; }
    }

    public ChestSlotInfo(int item_id, int count)
    {
        m_item_id = item_id;
        m_item_count = count;
    }
}
