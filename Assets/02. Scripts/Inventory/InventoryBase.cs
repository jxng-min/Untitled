using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
    [SerializeField] protected GameObject m_inventory_base;
    [SerializeField] protected GameObject m_inventory_slots_parent;
    [SerializeField] protected InventorySlot[] m_slots;
    public InventorySlot[] Slots
    {
        get { return m_slots; }
    }

    protected void Awake()
    {
        if(m_inventory_base.activeSelf)
        {
            m_inventory_base.SetActive(false);
        }

        m_slots = m_inventory_slots_parent.GetComponentsInChildren<InventorySlot>();
    }
}
