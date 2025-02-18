using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    [field: Header("아이템 상점의 고유 ID")]
    [field: SerializeField] public int ID { get; private set; } = 0;

    [Header("상점에서 판매할 아이템")]
    [SerializeField] private ItemShopSlotInfo[] m_sell_item_infos;
    public ItemShopSlotInfo[] SellingItem
    {
        get { return m_sell_item_infos; }
    }

    [SerializeField] private int m_shop_level = 0;
    public int Level { get; private set; }

    private void Awake()
    {
        List<ItemShopSlotInfo> item_shop_slot_infos = new List<ItemShopSlotInfo>();

        foreach(var shop_slot_info in SellingItem)
        {
            item_shop_slot_infos.Add(new ItemShopSlotInfo(shop_slot_info));
        }

        m_sell_item_infos = item_shop_slot_infos.ToArray();
    }

    public void LoadData(ShopInfo shop_info)
    {
        for(int i = 0; i < m_sell_item_infos.Length; i++)
        {
            m_sell_item_infos[i].Amount = shop_info.ItemInfos[i].m_amount;
        }

        Level = shop_info.Level;
    }

    public ShopInfo SaveData()
    {
        ShopInfo shop_info = new ShopInfo();

        shop_info.ID = ID;
        shop_info.Level = Level;
        shop_info.ItemInfos = SellingItem.Select(
                                                    sell_item_info => new SellItemInfo
                                                    {
                                                        m_amount = sell_item_info.Amount,
                                                        m_entity_code = (int)sell_item_info.Item.ID
                                                    }
                                                ).ToArray();

        return shop_info;
    }
}
