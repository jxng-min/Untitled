using UnityEngine;
using System.Collections.Generic;

public class DropItemManager : MonoBehaviour
{
    public Dictionary<ItemCode,ItemObject> m_all_dorp_item_dic= new Dictionary<ItemCode,ItemObject>();
    public List<ItemObject> m_item_object_list = new List<ItemObject>();
    void Start()
    {
        foreach(ItemObject item in m_item_object_list)
        {
            if (!m_all_dorp_item_dic.ContainsKey(item.item_code))
            {
                m_all_dorp_item_dic.Add(item.item_code, item);
            }
        }
    }

    public void AddItemToBag(List<ItemObject> bag,ItemCode[] codes, float[] chances)
    {
        for(int i = 0; i < codes.Length; i++) { 
            ItemObject item = m_all_dorp_item_dic[codes[i]].Clone();
            item.drop_chance = chances[i];
            bag.Add(item);
        }
    }

    public GameObject DropRandomItem(List<ItemObject> bag)
    {
        int rand = Random.Range(0, 101);

        float now_value = 0f;
        foreach(ItemObject item in bag)
        {
            now_value+= item.drop_chance;
            if(rand<now_value)
            {
                return item.item_prefab;
            }
        }
        return null;
    }
}
