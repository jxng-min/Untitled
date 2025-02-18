using UnityEngine;

public class Merchant : MonoBehaviour
{
    [field: Header("[아이템 상점 NPC]")]
    [Header("아이템 상점")]
    [SerializeField] private ItemShop m_item_shop;

    public void Trade()
    {
        ItemShopManager.Instance.OpenItemShop(m_item_shop.SellingItem, m_item_shop.Level);
    }
}
