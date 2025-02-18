using UnityEngine;

[System.Serializable]
public class ItemShopSlotInfo
{
    [Header("판매할 아이템")]
    [SerializeField] private Item m_sell_item;
    public Item Item
    {
        get { return m_sell_item; }
        set { m_sell_item = value; }
    }

    [Header("1회 거래 시의 아이템 가격")]
    [SerializeField] private int m_item_cost;
    public int Cost
    {
        get { return m_item_cost; }
        set { m_item_cost = value; }
    }

    [Header("아이템의 총 재고")]
    [SerializeField] private int m_item_amount;
    public int Amount
    {
        get { return m_item_amount; }
        set { m_item_amount = value; }
    }

    [Header("거래 1회 당 지급할 아이템의 개수")]
    [SerializeField] private int m_amount_per_trade;
    public int AmountPerTrade
    {
        get { return m_amount_per_trade; }
        set { m_amount_per_trade = value; }
    }

    [Header("상점의 진척도 단계")]
    [SerializeField] private int m_need_shop_level;
    public int Level
    {
        get { return m_need_shop_level; }
        set { m_need_shop_level = value; }
    }

    public ItemShopSlotInfo(ItemShopSlotInfo origin)
    {
        Item = origin.Item;
        Cost = origin.Cost;
        Amount = origin.Amount;
        AmountPerTrade = origin.AmountPerTrade;
        Level = origin.Level;
    }
}
