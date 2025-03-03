using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct SellItemInfo
{
    public int m_amount;
    public int m_entity_code;
}

[System.Serializable]
public struct ShopInfo
{
    public int ID;
    public int Level;
    public SellItemInfo[] ItemInfos;
}

[System.Serializable]
public struct ShopData
{
    public ShopInfo[] ShopInfos;
}

public class ItemShopManager : Singleton<ItemShopManager>
{
    private static bool m_is_active = false;
    public static bool IsActive
    {
        get { return m_is_active; }
    }

    [Header("상점 루트 오브젝트")]
    [SerializeField] private GameObject m_shop_root_object;

    [Header("상점 오브젝트의 프리팹 트랜스폼")]
    [SerializeField] private Transform m_slot_instantiate_transform;

    [Header("상점 슬롯 프리팹")]
    [SerializeField] private GameObject m_shop_slot_prefab;

    private List<ItemShopSlot> m_current_slots = new List<ItemShopSlot>();

    private string m_shop_data_path;

    private ItemShop[] m_item_shop;

    [SerializeField] private InventoryMain m_main_inventory;

    private new void Awake()
    {
        base.Awake();
        m_is_active = false;

        m_item_shop = FindObjectsByType<ItemShop>(FindObjectsSortMode.None);


        m_shop_data_path = Path.Combine(Application.persistentDataPath, "ShopData.json");
        LoadData();
    }

    private void LoadData()
    {
        if(File.Exists(m_shop_data_path))
        {
            var json_data = File.ReadAllText(m_shop_data_path);
            var shop_list = JsonUtility.FromJson<ShopData>(json_data);

            foreach(var shop in shop_list.ShopInfos)
            {
                for(int i = 0; i < m_item_shop.Length; i++)
                {
                    if(m_item_shop[i].ID == shop.ID)
                    {
                        m_item_shop[i].LoadData(shop);
                    } 
                }
            }
        }
        
        m_main_inventory.RefreshLabels();
    }

    public void SaveData()
    {
        List<ShopInfo> shop_info = new List<ShopInfo>(); 

        for(int i = 0; i < m_item_shop.Length; i++)
        {
            shop_info.Add(m_item_shop[i].SaveData());
        }

        ShopData shop_data;
        shop_data.ShopInfos = shop_info.ToArray();

        var json_data = JsonUtility.ToJson(shop_data);
        File.WriteAllText(m_shop_data_path, json_data);
    }

    public void OpenItemShop(ItemShopSlotInfo[] sell_items, int shop_level)
    {
        foreach(var item in sell_items)
        {
            var slot = Instantiate(m_shop_slot_prefab, Vector3.zero, Quaternion.identity, m_slot_instantiate_transform).GetComponent<ItemShopSlot>();
            slot.InitSlot(item, shop_level);

            m_current_slots.Add(slot);
        }

        m_shop_root_object.SetActive(true);
        m_is_active = true;

        RefreshSlots();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseItemShop()
    {
        foreach(var slot in m_current_slots)
        {
            Destroy(slot.gameObject);
        }

        m_current_slots.Clear();

        m_shop_root_object.SetActive(false);
        m_is_active = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SoundManager.Instance.PlayBGM(SoundManager.Instance.LastBGM);
    }


    public void RefreshSlots()
    {
        foreach(var slot in m_current_slots)
        {
            slot.RefreshSlot();
        }

        m_main_inventory.RefreshLabels();
    }
}
