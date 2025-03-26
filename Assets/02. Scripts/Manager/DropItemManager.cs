using UnityEngine;
using System.Collections.Generic;
using Junyoung;
using System.IO;
using UnityEngine.AI;

public class DropItemManager : MonoBehaviour
{
    public Dictionary<ItemCode,ItemObject> m_all_dorp_item_dic= new Dictionary<ItemCode,ItemObject>();
    public List<ItemObject> m_item_object_list = new List<ItemObject>();

    private string m_save_path;
    private GameObject m_global_object;

    private void Awake()
    {
        m_save_path = Application.persistentDataPath + "/DroppedItemsData.json";
        m_global_object = GameObject.Find("[Global]");
    }

    void Start()
    {
        foreach(ItemObject item in m_item_object_list)
        {
            if (!m_all_dorp_item_dic.ContainsKey(item.item_code))
            {
                m_all_dorp_item_dic.Add(item.item_code, item);
            }
        }
        if (!File.Exists(m_save_path))
        {
            Debug.Log("저장된 드랍된 아이템 데이터가 없음");
        }
        else
        {
            SpawnLoadItems();
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

    public void InitItemBag(List<ItemObject> bag)
    {
        bag.Clear();
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

    public GameObject DropRandomMoney(int min, int max, GameObject money_prefab)
    {
        int rand_money = Random.Range(min, max+1);

        money_prefab.GetComponent<ItemPickUp>().Item.Money = rand_money;


        return money_prefab;
    }

    public void SaveItems()
    {
        List<DropItemSaveData> save_data_list = new List<DropItemSaveData>();

        ItemPickUp[] drop_items = m_global_object.GetComponentsInChildren<ItemPickUp>();

        foreach (ItemPickUp item in drop_items)
        {
            DropItemSaveData data = new DropItemSaveData(((ItemCode)item.Item.ID),
                new SVector3(item.gameObject.transform.position), new SQuaternion(item.gameObject.transform.rotation), item.Item.Money);
            save_data_list.Add(data);
        }

        string json = JsonUtility.ToJson(new SWrapper<DropItemSaveData>(save_data_list), true);
        File.WriteAllText(m_save_path, json);
        Debug.Log(json);
    }

    public List<DropItemSaveData> LoadItems()
    {
        if (!File.Exists(m_save_path))
        {
            Debug.Log("저장된 드랍된 아이템 데이터가 없음");
            return null;
        }

        string json = File.ReadAllText(m_save_path);
        SWrapper<DropItemSaveData> swrapper = JsonUtility.FromJson<SWrapper<DropItemSaveData>>(json);
        Debug.Log($"드랍된 아이템 데이터 로드 완료");
        return swrapper.items;
    }

    public void SpawnLoadItems()
    {
        List<DropItemSaveData> items= LoadItems();

        foreach(DropItemSaveData i in items)
        {
            var drop_item = Instantiate(m_all_dorp_item_dic[(i.m_item_code)].item_prefab);
            drop_item.transform.position = i.m_position.ToVector3();
            drop_item.transform.rotation = i.m_rotation.ToQuaternion();
            drop_item.transform.SetParent(m_global_object.transform);
            drop_item.GetComponent<ItemPickUp>().Item.Money= i.m_money;

        }
    }

}
