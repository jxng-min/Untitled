using UnityEngine;

[System.Serializable]
public class ItemObject
{
    
    public ItemCode item_code; // 아이템 코드
    public GameObject item_prefab; // 아이템 프리팹
    public float drop_chance; // 드랍 확률 (0~100%)

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
