using UnityEngine;

public abstract class WeaponCtrl : MonoBehaviour
{
    [SerializeField] protected Weapon m_info;
    public Weapon Info { get { return m_info; } set { m_info = value; } }

    [SerializeField] protected BoxCollider m_area;
    [SerializeField] protected TrailRenderer m_trail_effect;

    public abstract void Use();
}
