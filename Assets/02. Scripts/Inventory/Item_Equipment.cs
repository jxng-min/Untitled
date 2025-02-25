using UnityEngine;

[CreateAssetMenu(fileName ="Equipment Item", menuName ="Scriptable Object/Item(Equipment)")]
public class Item_Equipment : Item
{
    [Space(50)] [Header("장비 아이템의 효과 (착용 시에 발생)")]
    [SerializeField] private EquipmentEffect m_effect;

    public EquipmentEffect Effect
    {
        get { return m_effect; }
    }
}