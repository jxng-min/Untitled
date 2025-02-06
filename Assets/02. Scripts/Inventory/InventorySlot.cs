using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Item m_item;
    public Item Item
    {
        get { return m_item; }
    }

    [Header("슬롯 타입 마스크")]
    [SerializeField] private ItemType m_slot_mask;

    private int m_item_count;

    [Header("아이템 슬롯에 있는 UI 오브젝트")]
    [SerializeField] private Image m_item_image;
    [SerializeField] private Image m_cool_time_image;
    [SerializeField] private TMP_Text m_text_count;

    private void SetColor(float alpha)
    {
        Color color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }

    public bool IsMask(Item item)
    {
        return ((int)item.Type & (int)m_slot_mask) == 0 ? false : true;
    }

    public void AddItem(Item item, int count = 1)
    {
        m_item = item;
        m_item_count = count;
        m_item_image.sprite = m_item.Image;

        if(m_item.Type <= ItemType.Equipment_SHOES)
        {
            m_text_count.text = "";
        }
        else
        {
            m_text_count.text = m_item_count.ToString();
        }

        SetColor(1f);
    }

    public void UpdateSlotCount(int count)
    {
        m_item_count += count;
        m_text_count.text = m_item_count.ToString();

        if(m_item_count <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        m_item = null;
        m_item_count = 0;
        m_item_image.sprite = null;
        SetColor(0f);

        m_text_count.text = "";
    }
}
