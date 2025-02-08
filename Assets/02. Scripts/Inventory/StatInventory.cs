using UnityEngine;
using TMPro;

public class StatInventory : InventoryBase
{
    public static bool Active { get; set; } = false;

    [Header("Player's Stats")]
    [SerializeField] private TMP_Text m_attack_label;
    [SerializeField] private TMP_Text m_attack_rate_label;
    [SerializeField] private TMP_Text m_defense_label;

    private new void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(Active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            if(m_inventory_base.activeInHierarchy)
            {
                m_inventory_base.SetActive(false);
                Active = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                m_inventory_base.SetActive(true);
                Active = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }        
    }
}
