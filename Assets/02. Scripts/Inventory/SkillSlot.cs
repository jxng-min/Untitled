using TMPro;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    [Header("스킬 슬롯의 이름 라벨")]
    [SerializeField] private TMP_Text m_skill_name_label;

    [Header("스킬 슬롯이 가지는 인벤토리 슬롯")]
    [SerializeField] private InventorySlot m_slot;

    [Header("비활성화 이미지 오브젝트")]
    [SerializeField] private GameObject m_disable_image;

    private int m_unlock_level;
    public int UnlockLV
    {
        get { return m_unlock_level; }
        set { m_unlock_level = value; }
    }

    private Item_Skill m_current_skill;
    private Item_Skill Skill
    {
        get { return m_current_skill; }
        set { m_current_skill = value; }
    }

    public void SetSkillTitle(string skill_name)
    {
        m_skill_name_label.text = skill_name;
    }

    public void Init(Item_Skill skill)
    {
        Skill = skill;

        gameObject.SetActive(true);

        m_slot.ClearSlot();
        m_slot.AddItem(skill);

        m_skill_name_label.text = skill.Name;

        UnlockLV = skill.UnlockLV;
        UpdateSkillState();
    }

    public void LockSlot(bool is_usable)
    {
        m_disable_image.SetActive(is_usable);
    }

    public void UpdateSkillState()
    {
        if(UnlockLV <= DataManager.Instance.Data.Level)
        {
            LockSlot(false);
        }
        else
        {
            LockSlot(true);
        }
    }
}
