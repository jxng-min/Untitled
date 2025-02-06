using System.Collections.Generic;
using UnityEngine;

public class ItemCooltimeManager : Singleton<ItemCooltimeManager>
{
    private Dictionary<int, float> m_cool_times;
    private List<int> m_cool_time_list;

    private float m_temp_cool_time;

    private void Start()
    {
        m_cool_times = new Dictionary<int, float>();
        m_cool_time_list = new List<int>();
    }

    private void Update()
    {
        for(int i = m_cool_time_list.Count - 1; i >= 0; i--)
        {
            m_temp_cool_time = m_cool_times[m_cool_time_list[i]] -= Time.deltaTime;

            if(m_temp_cool_time < 0f)
            {
                m_cool_time_list.RemoveAt(i);
            } 
        }
    }

    public void AddCooltimeQueue(int item_id, float origin_cool_time)
    {
        m_cool_times.TryAdd(item_id, origin_cool_time);

        m_cool_times[item_id] = origin_cool_time;
        m_cool_time_list.Add(item_id);
    }

    public float GetCurrentCooltime(int item_id)
    {
        float cool_time;
        bool is_success = m_cool_times.TryGetValue(item_id, out cool_time);

        if(is_success)
        {
            return cool_time;
        }
        else
        {
            return 0;
        }
    }
}
