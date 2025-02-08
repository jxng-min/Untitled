using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
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

    private ItemActionManager m_item_action_manager;
    private bool m_is_tool_tip_active = false;

    private ItemDescription m_tool_tip_script;

    private void Awake()
    {
        m_item_action_manager = GameObject.Find("Inventory Manager").GetComponent<ItemActionManager>();
        m_tool_tip_script = GetComponentInParent<ItemDescription>();
    }

    private void Update()
    {
        if(m_item is not null)
        {
            m_cool_time_image.fillAmount = ItemCooltimeManager.Instance.GetCurrentCooltime(m_item.ID) / m_item.Cooltime;
        }
        else
        {
            m_cool_time_image.fillAmount = 0f;
        }

        if(m_is_tool_tip_active)
        {
            m_tool_tip_script.OpenUI(m_item.ID);
            m_is_tool_tip_active = false;
        }
    }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(m_slot_mask == ItemType.SKILL)
            {
                return;
            }

            UseItem();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(m_item is not null)
        {
            DragSlot.Instance.GetComponent<RectTransform>().SetAsLastSibling();

            if(Input.GetKey(KeyCode.LeftShift))
            {
                DragSlot.Instance.m_is_shift_mode = true;
            }
            else
            {
                DragSlot.Instance.m_is_shift_mode = false;
            }

            DragSlot.Instance.m_current_slot = this;
            DragSlot.Instance.DragSetImage(m_item_image);
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(m_item is not null)
        {
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.Instance.SetColor(0f);
        DragSlot.Instance.m_current_slot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.Instance.m_is_shift_mode && m_item is not null)
        {
            return;
        }

        if(!IsMask(DragSlot.Instance.m_current_slot.Item))
        {
            return;
        }

        if(m_item is not null && !DragSlot.Instance.m_current_slot.IsMask(m_item))
        {
            return;
        }

        ChangeSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(m_item is not null)
        {
            m_is_tool_tip_active = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_tool_tip_script.CloseUI();
    }

    private void ChangeSlot()
    {
        if(DragSlot.Instance.m_current_slot.Item.Type >= ItemType.Etc)
        {
            if(m_item is not null && m_item.ID == DragSlot.Instance.m_current_slot.Item.ID)
            {
                int changed_slot_count;

                if(DragSlot.Instance.m_is_shift_mode)
                {
                    changed_slot_count = (int)(DragSlot.Instance.m_current_slot.m_item_count * 0.5f);
                }
                else
                {
                    changed_slot_count = DragSlot.Instance.m_current_slot.m_item_count;
                }

                UpdateSlotCount(changed_slot_count);
                DragSlot.Instance.m_current_slot.UpdateSlotCount(-changed_slot_count);
                return;
            }

            if(DragSlot.Instance.m_is_shift_mode)
            {
                int changed_slot_count = (int)(DragSlot.Instance.m_current_slot.m_item_count * 0.5f);

                if(changed_slot_count == 0)
                {
                    AddItem(DragSlot.Instance.m_current_slot.Item, 1);
                    DragSlot.Instance.m_current_slot.ClearSlot();
                    return;
                }

                AddItem(DragSlot.Instance.m_current_slot.Item, changed_slot_count);
                DragSlot.Instance.m_current_slot.UpdateSlotCount(-changed_slot_count);
                return;
            }
        }

        Item temp_item = m_item;
        int temp_item_count = m_item_count;

        AddItem(DragSlot.Instance.m_current_slot.Item, DragSlot.Instance.m_current_slot.m_item_count);

        if(temp_item is not null)
        {
            DragSlot.Instance.m_current_slot.AddItem(temp_item, temp_item_count);
        }
        else
        {
            DragSlot.Instance.m_current_slot.ClearSlot();
        }
    }

    public void UseItem()
    {
        if(m_item is not null)
        {
            if(m_item.Interactivity is false)
            {
                return;
            }

            if(ItemCooltimeManager.Instance.GetCurrentCooltime(m_item.ID) > 0f)
            {
                return;
            }

            if(!m_item_action_manager.UseItem(m_item))
            {
                return;
            }

            if(m_item.Cooltime > 0f)
            {
                ItemCooltimeManager.Instance.AddCooltimeQueue(m_item.ID, m_item.Cooltime);
            }

            if(m_item.Type >= ItemType.Equipment_HELMET && m_item.Type <= ItemType.Equipment_SHOES)
            {
                //ChangeEquipmentSlot();
            }

            if(m_item is not null && m_item.Consumable)
            {
                UpdateSlotCount(-1);
            }

            if(m_item is null)
            {
                //m_item_description.CloseUI();
            }
        }
    }
}
