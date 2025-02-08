using UnityEditor.ShortcutManagement;
using UnityEngine;

public class ShortcutManager : MonoBehaviour
{
    [SerializeField] private Transform m_slot_parent;

    private InventorySlot[] m_slots;

    private void Awake()
    {
        m_slots = m_slot_parent.GetComponentsInChildren<InventorySlot>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_slots[0].UseItem();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_slots[1].UseItem();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_slots[2].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {   
            m_slots[3].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            m_slots[4].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            m_slots[5].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            m_slots[6].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            m_slots[7].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            m_slots[8].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            m_slots[9].UseItem();
        }
        
        if(Input.GetKeyDown(KeyCode.H))
        {
            m_slots[10].UseItem();
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            m_slots[11].UseItem();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            m_slots[12].UseItem();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            m_slots[13].UseItem();
        }

        if(Input.GetKeyDown(KeyCode.Semicolon))
        {   
            m_slots[14].UseItem();
        }

        if(Input.GetKeyDown(KeyCode.DoubleQuote))
        {
            m_slots[15].UseItem();
        }
    }
}
