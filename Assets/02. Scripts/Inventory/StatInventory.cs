using UnityEngine;
using TMPro;
using Junyoung;

public class StatInventory : InventoryBase
{
    private static bool m_is_active = false;
    public static bool IsActive
    {
        get { return m_is_active; }
        private set { m_is_active = value; }
    }

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
        if(GameManager.Instance.GameState == GameEventType.PLAYING)
        {
            if(IsActive)
            {
                m_attack_label.text = DataManager.Instance.Data.Stat.ATK.ToString();
                m_attack_rate_label.text = DataManager.Instance.Data.Stat.Rate.ToString();
                m_defense_label.text = DataManager.Instance.Data.Stat.DEF.ToString();
            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                SoundManager.Instance.PlayEffect("Button Click");
                
                if(IsActive)
                {
                    IsActive = false;

                    m_inventory_base.SetActive(false);

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    IsActive = true;

                    GameManager.Instance.Player.ChangeState(PlayerState.IDLE);

                    m_inventory_base.SetActive(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
    }
}  