using UnityEngine;
using TMPro;

public class StatInventory : InventoryBase
{
    public static bool Active { get; set; } = false;

    [Header("Player's Stats")]
    [SerializeField] private TMP_Text m_attack_label;
    [SerializeField] private TMP_Text m_attack_rate_label;
    [SerializeField] private TMP_Text m_defense_label;

    private PlayerCtrl m_player_ctrl;

    private new void Awake()
    {
        base.Awake();

        m_player_ctrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    private void Update()
    {
        if(Active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            m_attack_label.text = m_player_ctrl.AttackPower.ToString();
            m_attack_rate_label.text = m_player_ctrl.AttackRate.ToString();
            m_defense_label.text = m_player_ctrl.Defense.ToString();
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
