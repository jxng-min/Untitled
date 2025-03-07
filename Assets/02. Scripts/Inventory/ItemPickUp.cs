using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [Header("해당 오브젝트에 할당되는 아이템")]
    [SerializeField] private Item m_item;
    public Item Item
    {
        get { return m_item; }
    }


    [Header("아이템 인디케이터")]
    [SerializeField] private float m_indicator_height;
    public float IndicatorHeight
    {
        get { return m_indicator_height; }
    }
}