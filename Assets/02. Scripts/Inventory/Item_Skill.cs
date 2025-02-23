using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Scriptable Object/Item(Skill)")]
public class Item_Skill : Item
{
    [Space(50)]
    [Header("스킬의 이름")]
    [SerializeField] private string m_skill_name;
    public string Name
    {
        get { return m_skill_name; }
    }

    [Header("스킬 해금 레벨")]
    [SerializeField] private int m_unlock_level;
    public int UnlockLV
    {
        get { return m_unlock_level; }
    }
}
