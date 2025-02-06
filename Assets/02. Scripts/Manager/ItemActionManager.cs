using UnityEngine;

public class ItemActionManager : MonoBehaviour
{
    public static string m_skill_message = "Active Skill";

    [SerializeField] private PlayerCtrl m_playerCtrl;

    [Header("Preloaded objects into the scene")]
    [SerializeField] private GameObject[] m_objects;

    public bool UseItem(Item item)
    {
        switch(item.Type)
        {
            case ItemType.SKILL:
                // TODO: 스킬 처리 추가
                break;
            
            case ItemType.Consumable:
                switch(item.ID)
                {
                    case (int)ItemCode.SMALL_HP_POTION:
                        m_playerCtrl.UpdateHP(20f);
                        break;
                    
                    case (int)ItemCode.SMALL_MP_POTION:
                        m_playerCtrl.UpdateMP(10f);
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

    public void SlotOnDropEvent(InventorySlot slot)
    {
        Debug.Log("SlotDropEvent");
    }
}
