using UnityEngine;

[System.Serializable]
public class ItemObject
{
    
    public ItemCode item_code; // ������ �ڵ�
    public GameObject item_prefab; // ������ ������
    public float drop_chance; // ��� Ȯ�� (0~100%)

    public ItemObject Clone()
    {
        return new ItemObject
        {
            item_code = this.item_code,
            item_prefab = this.item_prefab,
            drop_chance = this.drop_chance
        };
    }
}
