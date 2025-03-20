using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolInfo
{
    public ObjectType m_type;
    public int m_init_count;
    public GameObject m_Prefab;
    public GameObject m_container;
    public Queue<GameObject> m_pool_queue = new Queue<GameObject>();
}

public class ObjectManager : Singleton<ObjectManager>
{
    [SerializeField] private List<PoolInfo> m_pool_info_list;

    public void Initialize()
    {
        foreach(PoolInfo info in m_pool_info_list)
        {
            for(int i = 0; i < info.m_init_count; i++)
            {
                info.m_pool_queue.Enqueue(CreateNewObject(info));
            }
        }
    }

    private GameObject CreateNewObject(PoolInfo info)
    {
        GameObject new_obj = Instantiate(info.m_Prefab, info.m_container.transform);
        new_obj.SetActive(false);

        return new_obj;
    }

    private PoolInfo GetPoolByType(ObjectType type)
    {
        foreach(PoolInfo info in m_pool_info_list)
        {
            if(info.m_type == type)
            {
                return info;
            }
        }

        return null;
    }

    public GameObject GetObject(ObjectType type)
    {
        PoolInfo info = GetPoolByType(type);
        
        GameObject obj;
        if(info.m_pool_queue.Count > 0)
        {
            obj = info.m_pool_queue.Dequeue();
        }
        else
        {
            obj = CreateNewObject(info);
        }
        obj.SetActive(true);

        return obj;
    }

    public void ReturnObject(GameObject obj, ObjectType type)
    {
        PoolInfo info = GetPoolByType(type);

        if(info.m_pool_queue.Count < info.m_init_count)
        {
            info.m_pool_queue.Enqueue(obj);
            obj.SetActive(false);
        }
        else
        {
            Destroy(obj);
        }
    }
}
