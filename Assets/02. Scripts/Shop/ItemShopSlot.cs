using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ItemShopSlot : MonoBehaviour
{
    private InventoryMain m_main_inventory;
    [SerializeField] private InventorySlot m_item_slot;
    [SerializeField] private TMP_Text m_item_name_label;
    [SerializeField] private TMP_Text m_item_cost_label;
    [SerializeField] private Button m_buy_button;

    private ItemShopSlotInfo m_sell_info;
    private int m_called_shop_level;

    public void RefreshSlot()
    {
        if(m_sell_info.Level > m_called_shop_level)
        {
            m_item_cost_label.text = "<color=red>지금은 구매할 수 없습니다.</color>";
            m_buy_button.interactable = false;

            return;
        }

        if(DataManager.Instance.Data.Money < m_sell_info.Cost || m_sell_info.Amount <= 0)
        {
            m_buy_button.interactable = false;
        }
        else
        {
            m_buy_button.interactable = true;
        }

        m_item_cost_label.text = $"{m_sell_info.Cost} ({m_sell_info.Amount}개 남음)";
    }

    public void InitSlot(ItemShopSlotInfo sell_item, int shop_level)
    {
        m_main_inventory = GameObject.Find("Inventory Manager").GetComponent<InventoryMain>();

        m_called_shop_level = shop_level;
        m_sell_info = sell_item;

        m_item_slot.ClearSlot();

        m_main_inventory.LoadItem(m_sell_info.Item, m_item_slot, m_sell_info.AmountPerTrade);
        
        m_item_name_label.text = ItemDataManager.Instance.GetName(m_sell_info.Item.ID);
    }

    public void BTN_BuyItem()
    {
        DataManager.Instance.Data.Money -= m_sell_info.Cost;

        m_main_inventory.AcquireItem(m_sell_info.Item, m_sell_info.AmountPerTrade < m_sell_info.Amount ? m_sell_info.AmountPerTrade : m_sell_info.Amount);

        m_sell_info.Amount -= m_sell_info.AmountPerTrade;
        m_sell_info.Amount = Mathf.Clamp(m_sell_info.Amount, 0, int.MaxValue);

        ItemShopManager.Instance.RefreshSlots();

        ItemShopManager.Instance.SaveData();

        // TODO: 구매 사운드 출력
    }
}
