using System.Collections.Generic;
using System.IO;
using Junyoung;
using TMPro;
using UnityEngine;

public class ChestDataManager : Singleton<ChestDataManager>
{
    private static bool m_is_ui_active = false;
    public static bool IsActive
    {
        get { return m_is_ui_active; }
        private set { m_is_ui_active = value; }
    }

    private string m_chest_data_path;

    [Header("메인 인벤토리")]
    [SerializeField] private InventoryMain m_main_inventory;

    [Header("상자 오브젝트 목록")]
    [SerializeField] private Chest[] m_chest_list;

    [Header("상자 UI 오브젝트")]
    [SerializeField] private GameObject m_chest_ui_object;

    [Header("상자 UI 오브젝트의 이름 라벨")]
    [SerializeField] private TMP_Text m_chest_name_label;

    [Header("슬롯이 저장될 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_parent_transform;

    private InventorySlot[] m_chest_slots;

    private Chest m_current_chest;
    private Animator m_chest_animator;

    private new void Awake()
    {
        base.Awake();

        Init();
    }

    private void Init()
    {
        m_chest_data_path = Path.Combine(Application.persistentDataPath, "ChestData.json");

        if(File.Exists(m_chest_data_path))
        {
            LoadData();
        }
        else
        {
            InitAllChests();
        }

        m_chest_ui_object.SetActive(false);
        m_chest_slots = m_slot_parent_transform.GetComponentsInChildren<InventorySlot>();
    }

    public void TryOpenChestUI(int chest_id)
    {
        IsActive = true;
        m_current_chest = null;

        foreach(var chest in m_chest_list)
        {
            if(chest_id == chest.ID)
            {
                m_current_chest = chest;

                break;
            }
        }

        m_chest_animator = m_current_chest.GetComponent<Animator>();
        m_chest_animator.SetBool("IsOpen", true);

        m_chest_ui_object.SetActive(true);
        m_is_ui_active = true;

        m_chest_name_label.text = m_current_chest is not null ? m_current_chest.Name : "";

        for(int i = 0; i < m_chest_slots.Length; i++)
        {
            if(m_current_chest.SlotInfos[i].ID == (int)ItemCode.NONE)
            {
                continue;
            }
            var item = ItemDataManager.Instance.GetItemByID(m_current_chest.SlotInfos[i].ID);

            m_main_inventory.LoadItem(item, m_chest_slots[i], m_current_chest.SlotInfos[i].Count);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SoundManager.Instance.PlayEffect("Chest Open");
    }

    private void LoadData()
    {
        if(File.Exists(m_chest_data_path))
        {
            var json_data = File.ReadAllText(m_chest_data_path);
            var chest_data_list = JsonUtility.FromJson<ChestInfoList>(json_data);

            if(chest_data_list is not null)
            {
                foreach(var chest in m_chest_list)
                {
                    for(int i = 0; i < chest_data_list.m_chest_infos.Length; i++)
                    {
                        if(chest.ID == chest_data_list.m_chest_infos[i].ID)
                        {
                            chest.LoadData(chest_data_list.m_chest_infos[i]);
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"{m_chest_data_path}에서 ChestData.json을 불러오는 과정에서 오류가 발생했습니다.");
            }
        }
        else
        {
            Debug.Log($"{m_chest_data_path}가 존재하지 않습니다.");
        }
    }

    private void SaveData()
    {
        List<ChestInfo> chest_info_list = new List<ChestInfo>();

        foreach(var chest in m_chest_list)
        {
            chest_info_list.Add(chest.SaveData());
        }

        ChestInfoList chest_infos = new ChestInfoList(chest_info_list.ToArray());

        var json_data = JsonUtility.ToJson(chest_infos);
        File.WriteAllText(m_chest_data_path, json_data);
    }

    private void InitAllChests()
    {
        foreach(var chest in m_chest_list)
        {
            chest.Init();
        }
    }

    public void BTN_CloseUI()
    {
        IsActive = false;
        if(m_chest_ui_object.activeInHierarchy && m_is_ui_active && m_current_chest is not null)
        {
            for(int i = 0; i < m_chest_slots.Length; i++)
            {
                if(m_chest_slots[i].Item == null)
                {
                    m_current_chest.SlotInfos[i] = new ChestSlotInfo((int)ItemCode.NONE, 0);
                }
                else
                {
                    m_current_chest.SlotInfos[i] = new ChestSlotInfo(m_chest_slots[i].Item.ID, m_chest_slots[i].Count);
                }
            }
        }   

        SaveData();

        foreach(var slot in m_chest_slots)
        {
            slot.ClearSlot();
        }

        m_current_chest = null;

        m_chest_animator.SetBool("IsOpen", false);

        m_chest_ui_object.SetActive(false);
        m_is_ui_active = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SoundManager.Instance.PlayEffect("Chest Close");
    }
}