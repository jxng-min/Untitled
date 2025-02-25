using UnityEngine;

public class ShortcutManager : MonoBehaviour
{
    private static bool m_is_ui_active = true;
    public static bool IsActive
    {
        get { return m_is_ui_active; }
        set { m_is_ui_active = value; }
    }

    [Header("단축키 UI 오브젝트")]
    [SerializeField] private GameObject m_shortcut_ui_object;

    [Header("슬롯의 부모 프랜스폼")]
    [SerializeField] private Transform m_slot_parent;

    private InventorySlot[] m_slots;
    public InventorySlot[] Slots
    {
        get { return m_slots; }
    }

    private void Awake()
    {
        m_slots = m_slot_parent.GetComponentsInChildren<InventorySlot>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(!IsActive)
            {
                IsActive = true;
                m_shortcut_ui_object.SetActive(true);
            }
            else
            {
                IsActive = false;
                m_shortcut_ui_object.SetActive(false);
            }
        }


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_slots[0].UseItem();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_slots[1].UseItem();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_slots[2].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {   
            m_slots[3].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            m_slots[4].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            m_slots[5].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            m_slots[6].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            m_slots[7].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            m_slots[8].UseItem();
        }
    }

    public void LoadItem(Item item, InventorySlot target_slot, int count = 1)
    {
        target_slot.AddItem(item, count);
    }
}
