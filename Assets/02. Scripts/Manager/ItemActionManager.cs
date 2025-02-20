using UnityEngine;

public class ItemActionManager : MonoBehaviour
{
    public static string m_skill_message = "Active Skill";

    [SerializeField] private PlayerCtrl m_player_ctrl;

    [Header("Preloaded objects into the scene")]
    [SerializeField] private GameObject[] m_objects;

    private InventoryMain m_main_inventory;
    private EquipmentInventory m_equipment_inventory;

    private void Awake()
    {
        m_main_inventory = GetComponent<InventoryMain>();
        m_equipment_inventory = GetComponent<EquipmentInventory>();
    } 

    public bool UseItem(Item item, InventorySlot called_slot = null)
    {
        switch(item.Type)
        {
            case ItemType.SKILL:
                // TODO: 스킬 처리 추가
                break;
            
            case ItemType.Equipment_HELMET:
            case ItemType.Equipment_ARMORPLATE:
            case ItemType.Equipment_WEAPON:
            case ItemType.Equipment_SHIELD:
            case ItemType.Equipment_SHOES:
                if(Item.CheckEquipmentType(called_slot.SlotMask))
                {
                    var main_slot = m_main_inventory.IsCanAquireItem(item);

                    if(main_slot is not null)
                    {
                        called_slot.ClearSlot();
                        m_main_inventory.AcquireItem(item, main_slot);
                    }
                }
                else
                {
                    var equipment_slot = m_equipment_inventory.GetEquipmentSlot(item.Type);

                    Item temp_item = equipment_slot.Item;

                    equipment_slot.AddItem(item);

                    if(temp_item is not null)
                    {
                        called_slot.AddItem(temp_item);
                    }
                    else
                    {
                        called_slot.ClearSlot();
                    }
                }

                // TODO: 장비 착용 효과음 재생
                m_equipment_inventory.CalculateEffect();
                DataManager.Instance.UpdateStat();
                m_player_ctrl.UpdateAttackSpeed();

                return false;

            case ItemType.Consumable:
                Debug.Log("사용함");
                switch(item.ID)
                {
                    case (int)ItemCode.SMALL_HP_POTION:
                        m_player_ctrl.UpdateHP(20f);
                        break;
                    
                    case (int)ItemCode.SMALL_MP_POTION:
                        m_player_ctrl.UpdateMP(10f);
                        break;
                }
                break;
            
            case ItemType.Consumable | ItemType.Ingredient:
                switch(item.ID)
                {
                    case (int)ItemCode.MEAT:
                        m_player_ctrl.UpdateHP(5f);
                        break;
                    
                    case (int)ItemCode.APPLE:
                        m_player_ctrl.UpdateMP(5f);
                        break;
                }
                break;
        }

        return true;
    }

    public void InteractionItem(Item item, GameObject interact_target)
    {
        if(interact_target.CompareTag("NPC"))
        {
            // ...
        }
    }

    public void SlotOnDropEvent(InventorySlot from_slot, InventorySlot to_slot)
    {
        if(Item.CheckEquipmentType(from_slot.SlotMask) || Item.CheckEquipmentType(to_slot.SlotMask))
        {
            // TODO: 장비 착용 효과음 재생
            m_equipment_inventory.CalculateEffect();
            DataManager.Instance.UpdateStat();
            m_player_ctrl.UpdateAttackSpeed();
        }
    }
}
