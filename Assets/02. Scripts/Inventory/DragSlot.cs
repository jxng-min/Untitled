using UnityEngine;
using UnityEngine.UI;

public class DragSlot : Singleton<DragSlot>
{
    [HideInInspector] public InventorySlot m_current_slot;
    [HideInInspector] public bool m_is_shift_mode;
    [SerializeField] private Image m_item_image;

    public void DragSetImage(Image item_image)
    {
        m_item_image.sprite = item_image.sprite;
        SetColor(1f);
    }

    public void SetColor(float alpha)
    {
        Color color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }
}
