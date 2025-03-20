using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private string m_player_data_path;
    private PlayerData m_player_data;
    public PlayerData Data
    {
        get { return m_player_data; }
        set { m_player_data = value; }
    }

    private Stat DefaultStat { get; set; }
    private Stat GrowthStat { get; set; }

    private InventoryMain m_main_inventory;
    private EquipmentInventory m_equipment_inventory;
    private ShortcutManager m_quick_inventory;

    public PlayerStatusCtrl StatusUI { get; private set; }

    private new void Awake()
    {
        base.Awake();
        
        DefaultStat = new Stat(50f, 30f, 20f, 0.9f, 1f);
        GrowthStat = new Stat(10f, 10f, 5f, 0f, 0.5f);

        m_player_data_path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
    }

    public void Initialize()
    {
        m_main_inventory = GameObject.Find("Inventory Manager").GetComponent<InventoryMain>();
        m_equipment_inventory = GameObject.Find("Inventory Manager").GetComponent<EquipmentInventory>();
        m_quick_inventory = GameObject.Find("Inventory Manager").GetComponent<ShortcutManager>();

        if(File.Exists(m_player_data_path))
        {
            LoadPlayerData();

            if(Data.Stat.HP <= 0f)
            {
                Data.Stat.HP = GetMaxStat().HP;
                Data.Stat.MP = GetMaxStat().MP;

                Data.Position = new Vector3(101f, 0f ,15f);

                SavePlayerData();
            }

            UpdateStat();
        }
        else
        {
            Data = new PlayerData();

            SavePlayerData();
        }

        StatusUI = GetComponent<PlayerStatusCtrl>();

        LoadInventory();
    }

    public void SaveInventory()
    {
        for(int i = 0; i < m_main_inventory.Slots.Length; i++)
        {
            if(m_main_inventory.Slots[i].Item is null)
            {
                Data.m_main_map[i] = new Map { m_item_code = ItemCode.NONE, m_item_count = m_main_inventory.Slots[i].Count };
            }
            else
            {
                Data.m_main_map[i] = new Map { m_item_code = (ItemCode)m_main_inventory.Slots[i].Item.ID, m_item_count = m_main_inventory.Slots[i].Count };
            }
        }

        for(int i = 0; i < m_equipment_inventory.Slots.Length; i++)
        {
            if(m_equipment_inventory.Slots[i].Item is null)
            {
                Data.m_equipment_map[i] = new Map { m_item_code = ItemCode.NONE, m_item_count = m_equipment_inventory.Slots[i].Count };
            }
            else
            {
                Data.m_equipment_map[i] = new Map { m_item_code = (ItemCode)m_equipment_inventory.Slots[i].Item.ID, m_item_count = m_equipment_inventory.Slots[i].Count };
            }
        }

        for(int i = 0; i < m_quick_inventory.Slots.Length; i++)
        {
            if(m_quick_inventory.Slots[i].Item is null)
            {
                Data.m_quick_map[i] = new Map { m_item_code = ItemCode.NONE, m_item_count = m_quick_inventory.Slots[i].Count };
            }
            else
            {
                Data.m_quick_map[i] = new Map { m_item_code = (ItemCode)m_quick_inventory.Slots[i].Item.ID, m_item_count = m_quick_inventory.Slots[i].Count };
            }
        }
    }

    public void LoadInventory()
    {
        for(int i = 0; i < Data.m_main_map.Length; i++)
        {
            if(Data.m_main_map[i].m_item_code == ItemCode.NONE)
            {
                m_main_inventory.Slots[i].ClearSlot();
            }
            else
            {
                var item = GetItemByCode(Data.m_main_map[i].m_item_code);

                if(item is not null)
                {
                    m_main_inventory.LoadItem(item, m_main_inventory.Slots[i], Data.m_main_map[i].m_item_count);
                }
            }
        }

        for(int i = 0; i < Data.m_equipment_map.Length; i++)
        {
            if(Data.m_equipment_map[i].m_item_code == ItemCode.NONE)
            {
                m_equipment_inventory.Slots[i].ClearSlot();
            }
            else
            {
                var item = GetItemByCode(Data.m_equipment_map[i].m_item_code);

                if(item is not null)
                {
                    m_equipment_inventory.LoadItem(item, m_equipment_inventory.Slots[i], Data.m_equipment_map[i].m_item_count);
                }
            }
        }

        m_equipment_inventory.CalculateEffect();
        UpdateStat();

        for(int i = 0; i < Data.m_quick_map.Length; i++)
        {
            if(Data.m_quick_map[i].m_item_code == ItemCode.NONE)
            {
                m_quick_inventory.Slots[i].ClearSlot();
            }
            else
            {
                var item = GetItemByCode(Data.m_quick_map[i].m_item_code);

                if(item is not null)
                {
                    Debug.Log(item.ID);
                    m_quick_inventory.LoadItem(item, m_quick_inventory.Slots[i], Data.m_quick_map[i].m_item_count);
                }
            }
        }
    }

    public void SavePlayerData()
    {
        var json_data = JsonUtility.ToJson(Data);
        File.WriteAllText(m_player_data_path, json_data);
        Debug.Log("저장");
    }

    public void LoadPlayerData()
    {
        var json_data = File.ReadAllText(m_player_data_path);
        var player_data = JsonUtility.FromJson<PlayerData>(json_data);

        if(player_data is not null)
        {
            Data = player_data;
        }
    }

    public Stat GetMaxStat()
    {
        return DefaultStat + (GrowthStat * (Data.Level - 1)) + m_equipment_inventory.CurrentEquipmentEffect;
    }

    public float GetMaxExp(int level)
    {
        return 5 * level;
    }

    public void UpdateStat()
    {
        var temp_hp = Data.Stat.HP;
        var temp_mp = Data.Stat.MP;

        Data.Stat = GetMaxStat();
        Data.Stat.HP = temp_hp;
        Data.Stat.MP = temp_mp;
    }

    private Item GetItemByCode(ItemCode item_code)
    {
        foreach(var item in ItemDataManager.Instance.ItemObject)
        {
            if(item.ID == (int)item_code)
            {
                return item;
            }
        }

        return null;
    }
}