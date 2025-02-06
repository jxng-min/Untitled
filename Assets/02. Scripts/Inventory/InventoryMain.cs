using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMain : InventoryBase
{
    public static bool Active { get; set; } = false;

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
                return;
            }
        }
    }

    public InventorySlot[] GetAllItems()
    {
        return m_slots;
    }
}
