using UnityEngine;

public class EquipmentInventory : InventoryBase
{
    public static bool Active { get; set; } = false;

    private EquipmentEffect m_current_equipment_effect;

    public EquipmentEffect CurrentEquipmentEffect
    {
        get { return m_current_equipment_effect; }
    }

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

        if(Input.GetKeyDown(KeyCode.U))
        {
            if(m_inventory_base.activeInHierarchy)
            {
                m_inventory_base.SetActive(false);
                Active = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                m_inventory_base.SetActive(true);
                Active = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void CalculateEffect()
    {
        EquipmentEffect calculated_effect = new EquipmentEffect();

        foreach(var slot in m_slots)
        {
            if(slot.Item is null)
            {
                continue;
            }

            calculated_effect += ((Item_Equipment)slot.Item).Effect;
        }

        m_current_equipment_effect = calculated_effect;
    }

    public InventorySlot GetEquipmentSlot(ItemType type)
    {
        switch(type)
        {
            case ItemType.Equipment_HELMET:
                return m_slots[0];
            
            case ItemType.Equipment_ARMORPLATE:
                return m_slots[1];
            
            case ItemType.Equipment_WEAPON:
                return m_slots[2];
            
            case ItemType.Equipment_SHIELD:
                return m_slots[3];
            
            case ItemType.Equipment_SHOES:
                return m_slots[4];
        }

        return null;
    }
}
