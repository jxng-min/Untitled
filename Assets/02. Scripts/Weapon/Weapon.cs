using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Object/Create Weapon", order = int.MaxValue)]
public class Weapon : ScriptableObject
{
    [SerializeField] private WeaponType m_weapon_type;
    public WeaponType Type { 
        get { return m_weapon_type; } 
        set { m_weapon_type = value; } 
    }

    [SerializeField] private float m_weapon_damage;
    public float Damage {
        get { return m_weapon_damage; }
        set { m_weapon_damage = value; }
    }

    [SerializeField] private float m_weapon_rate;
    public float Rate {
        get { return m_weapon_rate; }
        set { m_weapon_rate = value;}
    }
}
