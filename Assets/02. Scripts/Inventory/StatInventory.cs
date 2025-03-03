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
        if(!SettingManager.IsActive)
        {
            if(Active)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                
                m_attack_label.text = DataManager.Instance.Data.Stat.ATK.ToString();
                m_attack_rate_label.text = DataManager.Instance.Data.Stat.Rate.ToString();
                m_defense_label.text = DataManager.Instance.Data.Stat.DEF.ToString();
            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                SoundManager.Instance.PlayEffect("Button Click");
                
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
}
