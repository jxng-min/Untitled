using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class NPCInfo
{
    public int m_npc_id;
    public string m_npc_name;
}

[System.Serializable]
public class NPCInfoList
{
    public NPCInfo[] m_npc_infos;
}

public class NPCDataManager : Singleton<NPCDataManager>
{
    private string m_npc_data_path;
    private Dictionary<int, string> m_npc_name_dics;

    private new void Awake()
    {
        base.Awake();

        m_npc_data_path = Path.Combine(Application.persistentDataPath, "NPCData.json");
        m_npc_name_dics = new Dictionary<int, string>();
    }

    public void Initialize()
    {
        m_npc_name_dics.Clear();
        
        if(File.Exists(m_npc_data_path))
        {
            var json_data = File.ReadAllText(m_npc_data_path);
            var npc_list = JsonUtility.FromJson<NPCInfoList>(json_data);

            if(npc_list is not null && npc_list.m_npc_infos is not null)
            {
                foreach(var npc in npc_list.m_npc_infos)
                {
                    m_npc_name_dics.Add(npc.m_npc_id, npc.m_npc_name);
                }
            }
            else
            {
                Debug.Log("Json format is incorrect or empty.");
            }
        }
        else
        {
            Debug.Log($"{m_npc_data_path} is not existed.");
        }
    }

    public string GetName(int item_id)
    {
        return m_npc_name_dics.ContainsKey(item_id) ? m_npc_name_dics[item_id] : null;
    }
}