using UnityEngine;

[System.Serializable]
public class DropItemSaveData 
{
    public ItemCode m_item_code;
    public SVector3 m_position;
    public SQuaternion m_rotation;
    public int m_money;

    public DropItemSaveData(ItemCode item_code, SVector3 position, SQuaternion rotation, int money)
    {
        m_item_code = item_code;
        m_position = position;
        m_rotation = rotation;
        m_money = money;
    }
}
