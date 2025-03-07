using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Create Item", order = int.MaxValue)]
public class Item : ScriptableObject
{
    [Header("아이템의 고유한 아이디")]
    [SerializeField] private int m_item_id;
    public int ID
    {
        get { return m_item_id; }
    }

    [Header("아이템의 중첩 유무")]
    [SerializeField] bool m_can_overlap;
    public bool Overlap
    {
        get { return m_can_overlap; }
    }

    [Header("사용 아이템의 유무")]
    [SerializeField] bool m_is_interactivity;
    public bool Interactivity
    {
        get { return m_is_interactivity; }
    }

    [Header("아이템 소비의 유무")]
    [SerializeField] bool m_is_consumable;
    public bool Consumable
    {
        get { return m_is_consumable; }
    }

    [Header("아이템의 쿨타임")]
    [SerializeField] float m_item_cooltime = -1;
    public float Cooltime
    {
        get { return m_item_cooltime; }
    }

    [Header("아이템의 타입")]
    [SerializeField] ItemType m_item_type;
    public ItemType Type
    {
        get { return m_item_type; }
    }

    [Header("아이템의 이미지")]
    [SerializeField] Sprite m_item_sprite;
    public Sprite Image
    {
        get { return m_item_sprite; }
    }

    [Header("금액")]
    [SerializeField] int m_money;
    public int Money
    {
        get { return m_money; }
        set { m_money = value; }
    }

    public static bool CheckEquipmentType(ItemType type)
    {
        return type >= ItemType.Equipment_HELMET && type <= ItemType.Equipment_SHOES;
    }
}
