using System.Data;
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

    [SerializeField] InventoryMain m_main_inventory;
    [SerializeField] EquipmentInventory m_equipment_inventory;

    public PlayerStatusCtrl StatusUI { get; private set; }

    private new void Awake()
    {
        base.Awake();
        
        DefaultStat = new Stat(50f, 30f, 20f, 0.9f, 1f);
        GrowthStat = new Stat(10f, 10f, 5f, 0f, 0.5f);

        m_player_data_path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        
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
        }
        else
        {
            Data = new PlayerData();

            SavePlayerData();
        }

        StatusUI = GetComponent<PlayerStatusCtrl>();
    }

    public void SavePlayerData()
    {
        var json_data = JsonUtility.ToJson(Data);
        File.WriteAllText(m_player_data_path, json_data);
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
}