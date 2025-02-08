using System;
using UnityEngine;

[Serializable] public struct EquipmentEffect
{
    [Header("추가 공격력")]
    [SerializeField] private float m_atk;
    public float ATK
    {
        get { return m_atk; }
    }

    [Header("추가 공격 속도")]
    [SerializeField] private float m_atk_rate;
    public float Rate
    {
        get { return m_atk_rate; }
    }

    [Header("추가 방어력")]
    [SerializeField] private float m_defense;
    public float DEF
    {
        get { return m_defense; }
    }

    public static EquipmentEffect operator+(EquipmentEffect v1, EquipmentEffect v2)
    {
        EquipmentEffect calculated_effect;

        calculated_effect.m_atk = v1.m_atk + v2.m_atk;
        calculated_effect.m_atk_rate = v1.m_atk_rate + v2.m_atk_rate;
        calculated_effect.m_defense = v1.m_defense + v2.m_defense;

        return calculated_effect;
    }
}

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