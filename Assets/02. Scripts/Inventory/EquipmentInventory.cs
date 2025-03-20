using Junyoung;
using UnityEngine;

public class EquipmentInventory : InventoryBase
{
    private static bool m_is_active = false;
    public static bool IsActive
    {
        get { return m_is_active; }
        private set { m_is_active = value; }
    }

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
        if(GameManager.Instance.GameState == GameEventType.PLAYING)
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                SoundManager.Instance.PlayEffect("Button Click");
                
                if(IsActive)
                {
                    IsActive = false;

                    m_inventory_base.SetActive(false);

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    IsActive = true;

                    m_inventory_base.SetActive(true);
                    
                    GameManager.Instance.Player.ChangeState(PlayerState.IDLE);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
    }

    public void LoadItem(Item item, InventorySlot target_slot, int count = 1)
    {
        target_slot.AddItem(item, count);
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
