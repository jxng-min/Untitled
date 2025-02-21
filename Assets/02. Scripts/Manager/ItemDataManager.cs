using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    public int m_item_id;
    public string m_item_name;
    public string m_item_description;
}

[System.Serializable]
public class ItemInfoList
{
    public ItemInfo[] m_item_infos;
}

public class ItemDataManager : Singleton<ItemDataManager>
{
    private string m_item_data_path;
    private Dictionary<int, string> m_item_name_dics;
    private Dictionary<int, string> m_item_description_dics;

    [SerializeField] private Item[] m_item_object_data;
    public Item[] ItemObject
    {
        get { return m_item_object_data; }
    }

    private void Start()
    {
        m_item_data_path = Path.Combine(Application.persistentDataPath, "ItemData.json");

        m_item_name_dics = new Dictionary<int, string>();
        m_item_description_dics = new Dictionary<int, string>();

        ParsingJson();
    }

    private void ParsingJson()
    {
        if(File.Exists(m_item_data_path))
        {
            var json_data = File.ReadAllText(m_item_data_path);
            var item_list = JsonUtility.FromJson<ItemInfoList>(json_data);

            if(item_list is not null && item_list.m_item_infos is not null)
            {
                foreach(var item in item_list.m_item_infos)
                {
                    m_item_name_dics.Add(item.m_item_id, item.m_item_name);
                    m_item_description_dics.Add(item.m_item_id, item.m_item_description);
                }
            }
            else
            {
                Debug.Log("Json format is incorrect or empty.");
            }
        }
        else
        {
            Debug.Log($"{m_item_data_path} is not existed.");
        }
    }

    public string GetName(int item_id)
    {
        return m_item_name_dics.ContainsKey(item_id) ? m_item_name_dics[item_id] : null;
    }

    public string GetDescription(int item_id)
    {
        return m_item_name_dics.ContainsKey(item_id) ? m_item_description_dics[item_id] : null;
    }

    public Item GetItemByID(int item_id)
    {
        foreach(var item in m_item_object_data)
        {
            if(item.ID == item_id)
            {
                return item;
            }
        }

        return null;
    }
}
