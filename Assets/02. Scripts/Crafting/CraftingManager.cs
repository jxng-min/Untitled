using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : Singleton<CraftingManager>
{
    private static bool m_is_ui_active = false;
    public static bool IsActive
    {
        get { return m_is_ui_active; }
    }

    [Header("제작소에 상관 없이 전역으로 사용 가능한 레시피 목록")]
    [SerializeField] private CraftingRecipe[] m_global_recipes;

    [Header("전역 레시피를 사용하지 않을 때 슬롯들을 임시로 보관할 트랜스폼")]
    [SerializeField] private Transform m_global_recipes_temp_transform;

    [Header("제작소 UI 오브젝트")]
    [SerializeField] private GameObject m_crafting_ui_object;

    [Header("제작소의 Content 트랜스폼")]
    [SerializeField] private Transform m_recipe_content_transform;

    [Header("제작소 슬롯 프리팹")]
    [SerializeField] private GameObject m_slot_prefab;

    [Header("제작 가능한 레시피들만 활성화하는 토글")]
    [SerializeField] private Toggle m_view_can_craft_toggle;

    [Header("제작소 UI 이름 라벨")]
    [SerializeField] private TMP_Text m_ui_name_label;

    [Header("메인 인벤토리")]
    [SerializeField] private InventoryMain m_main_inventory;

    CraftingSlot[] m_global_recipe_slots;
    List<CraftingSlot> m_local_recipe_slots = new List<CraftingSlot>();

    private int m_current_crafting_count;

    private new void Awake()
    {
        base.Awake();

        m_is_ui_active = false;
        Init();
    } 

    private void Init()
    {
        List<CraftingSlot> global_recipe_slots = new List<CraftingSlot>();

        foreach(var recipe in m_global_recipes)
        {
            CraftingSlot crafting_slot = Instantiate(m_slot_prefab, Vector3.zero, Quaternion.identity, m_global_recipes_temp_transform).GetComponent<CraftingSlot>();
            crafting_slot.Init(recipe);

            global_recipe_slots.Add(crafting_slot);
        }

        m_global_recipe_slots = global_recipe_slots.ToArray();
    }

    public void TryOpenCraftingUI(CraftingRecipe[] recipes, bool use_global_recipe, string title)
    {
        if(m_is_ui_active)
        {
            return;
        }

        for(int i = m_local_recipe_slots.Count; i < recipes.Length; i++)
        {
            CraftingSlot crafting_slot = Instantiate(m_slot_prefab, Vector3.zero, Quaternion.identity, m_recipe_content_transform).GetComponent<CraftingSlot>();
            m_local_recipe_slots.Add(crafting_slot);
        }

        for(int i = 0; i < m_local_recipe_slots.Count; i++)
        {
            if(i < recipes.Length)
            {
                m_local_recipe_slots[i].Init(recipes[i]);
            }
            else
            {
                m_local_recipe_slots[i].gameObject.SetActive(false);
            }
        }

        if(use_global_recipe)
        {
            foreach(var global_recipe in m_global_recipe_slots)
            {
                global_recipe.transform.SetParent(m_recipe_content_transform);
            }
        }

        m_crafting_ui_object.SetActive(true);

        m_ui_name_label.text = title;
        m_current_crafting_count = recipes.Length;
        m_is_ui_active = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        RefreshAllSlots();
    }

    public void CloseCraftingUI()
    {
        foreach(var global_recipe in m_global_recipe_slots)
        {
            global_recipe.transform.SetParent(m_global_recipes_temp_transform);
        }

        m_crafting_ui_object.SetActive(false);
        m_is_ui_active = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CheckCraftingSlot(CraftingSlot crafting_slot)
    {
        crafting_slot.gameObject.SetActive(true);

        for(int i = 0; i < crafting_slot.CurrentRecipe.RequireItems.Length; i++)
        {
            if(m_main_inventory.HasItemInInventory(crafting_slot.CurrentRecipe.RequireItems[i].Item.ID, out _, crafting_slot.CurrentRecipe.RequireItems[i].Count) == false)
            {
                if(m_view_can_craft_toggle.isOn)
                {
                    Debug.Log("제작할 수 없기 때문에 생략합니다.");
                    crafting_slot.gameObject.SetActive(false);
                }
                else
                {
                    crafting_slot.ToggleSlotState(false);
                }

                return;
            }
        }

        crafting_slot.ToggleSlotState(true);
    }

    public void RefreshAllSlots()
    {
        if(m_is_ui_active is false)
        {
            return;
        }

        for(int i = 0; i < m_current_crafting_count; i++)
        {
            CheckCraftingSlot(m_local_recipe_slots[i]);
        }

        if(m_global_recipes_temp_transform.childCount == 0)
        {
            foreach(var global_recipe_slot in m_global_recipe_slots)
            {
                CheckCraftingSlot(global_recipe_slot);
            }
        }
    }

    public void TOGGLE_ViewCraftable()
    {
        RefreshAllSlots();
    }
}
