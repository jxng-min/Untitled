using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private static bool m_is_ui_active = false;
    public static bool IsActive
    {
        get { return m_is_ui_active; }
    }

    [Header("모든 스킬 목록")]
    [SerializeField] private Item_Skill[] m_skill_list;

    [Header("스킬 슬롯 프리펩")]
    [SerializeField] private GameObject m_skill_slot_prefab;

    [Header("스킬 UI 오브젝트")]
    [SerializeField] private GameObject m_skill_ui_object;

    [Header("스킬 UI의 슬롯 부모 트랜스폼")]
    [SerializeField] private Transform m_skill_slot_root;

    private SkillSlot[] m_skill_slots;

    private new void Awake()
    {
        base.Awake();

        m_is_ui_active = false;
        m_skill_ui_object.SetActive(false);
    }

    private void Start()
    {
        Init();   
    }

    private void Update()
    {
        if(!SettingManager.IsActive)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                SoundManager.Instance.PlayEffect("Button Click");
                
                if(m_is_ui_active)
                {
                    m_skill_ui_object.SetActive(false);
                    m_is_ui_active = false;

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    m_skill_ui_object.SetActive(true);
                    m_is_ui_active = true;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
    }

    private void Init()
    {
        List<SkillSlot> skill_slot_list = new List<SkillSlot>();

        for(int i = 0; i < m_skill_list.Length; i++)
        {
            SkillSlot skill_slot = Instantiate(m_skill_slot_prefab, Vector3.zero, Quaternion.identity, m_skill_slot_root).GetComponent<SkillSlot>();
            skill_slot.Init(m_skill_list[i]);

            skill_slot_list.Add(skill_slot);
        }

        m_skill_slots = skill_slot_list.ToArray();
    }

    public void UpdateAllSlots()
    {
        foreach(var skill_slot in m_skill_slots)
        {
            skill_slot.UpdateSkillState();
        }
    }
}
