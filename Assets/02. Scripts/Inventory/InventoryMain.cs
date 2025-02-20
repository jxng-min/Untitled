using TMPro;
using UnityEngine;

public class InventoryMain : InventoryBase
{
    public static bool Active { get; set; } = false;

    [SerializeField] private TMP_Text m_money_label;

    private new void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(Active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!Active)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        m_inventory_base.SetActive(true);
        Active = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseInventory()
    {
        m_inventory_base.SetActive(false);
        Active = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadItem(Item item, InventorySlot target_slot, int count = 1)
    {
        target_slot.AddItem(item, count);
    }

    public void AcquireItem(Item item, InventorySlot target_slot, int count = 1)
    {
        if(item.Overlap)
        {
            if(target_slot.Item != null && target_slot.IsMask(item))
            {
                if(target_slot.Item.ID == item.ID)
                {
                    target_slot.UpdateSlotCount(count);
                }
            }
        }
        else
        {
            target_slot.AddItem(item, count);
        }

        DataManager.Instance.SaveInventory();
        DataManager.Instance.SavePlayerData(); // TODO: 나중에는 지워야 함.
    }

    public void AcquireItem(Item item, int count = 1)
    {
        if(item.Overlap)
        {
            for(int i = 0; i < m_slots.Length; i++)
            {
                if(m_slots[i].Item != null && m_slots[i].IsMask(item))
                {
                    if(m_slots[i].Item.ID == item.ID)
                    {
                        m_slots[i].UpdateSlotCount(count);

                        DataManager.Instance.SaveInventory();
                        DataManager.Instance.SavePlayerData(); // TODO: 나중에는 지워야 함.

                        return;
                    }
                }
            }    
        }

        for(int i = 0; i < m_slots.Length; i++)
        {
            if(m_slots[i].Item == null && m_slots[i].IsMask(item))
            {
                m_slots[i].AddItem(item, count);

                DataManager.Instance.SaveInventory();
                DataManager.Instance.SavePlayerData(); // TODO: 나중에는 지워야 함.
                
                return;
            }
        }
    }

    public InventorySlot[] GetAllItems()
    {
        return m_slots;
    }

    public InventorySlot IsCanAquireItem(Item item)
    {
        foreach(var slot in m_slots)
        {
            if(item.Overlap && slot.Item.Type == item.Type)
            {
                return slot;
            }

            if(slot.Item is null)
            {
                return slot;
            }
        }

        return null;
    }

    public void RefreshLabels()
    {
        m_money_label.text = "보유금: " + DataManager.Instance.Data.Money.ToString();
    }

    public bool HasItemInInventory(int item_id, out InventorySlot item_slot, int count)
    {
        item_slot = null;

        for(int i = 0; i < m_slots.Length; i++)
        {
            if(m_slots[i].Item is null)
            {
                continue;
            }
            
            if(m_slots[i].Item.ID == item_id)
            {
                if(m_slots[i].Count >= count)
                {
                    item_slot = m_slots[i];

                    return true;
                }
            }
        }

        return false;
    }
}
