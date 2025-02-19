using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    private static bool m_is_crafting = false;
    public static bool Crafting
    {
        get { return m_is_crafting; }
    }

    [Header("제작 대상 아이템 슬롯")]
    [SerializeField] private InventorySlot m_result_item_slot;

    [Header("재료 아이템을 담는 슬라이드")]
    [SerializeField] private Transform m_recipe_content_transform;

    [Header("제작 버튼")]
    [SerializeField] private Button m_crafting_button;

    [Header("제작 시간 텍스트")]
    [SerializeField] private TMP_Text m_crafting_time_label;

    [Header("제작 진행도 이미지")]
    [SerializeField] private Image m_crafting_progress_image;

    [Header("비활성화 상태 시, 보여줄 이미지 오브젝트")]
    [SerializeField] private GameObject m_disable_image_object;

    private CraftingRecipe m_current_recipe;
    public CraftingRecipe CurrentRecipe
    {
        get { return m_current_recipe; }
        set { m_current_recipe = value; }
    }

    private Coroutine m_coroutine_craft;
    private InventoryMain m_main_inventory;

    private void OnDisable()
    {
        if(m_coroutine_craft is not null)
        {
            StopCoroutine(m_coroutine_craft);
        }

        m_is_crafting = false;
    }

    public void Init(CraftingRecipe recipe)
    {
        CurrentRecipe = recipe;

        gameObject.SetActive(true);

        m_result_item_slot.ClearSlot();

        m_main_inventory = GameObject.Find("Inventory Manager").GetComponent<InventoryMain>();
        m_main_inventory.LoadItem(recipe.ResultItem.Item, m_result_item_slot, recipe.ResultItem.Count);

        m_crafting_time_label.text = $"{recipe.CraftingTime.ToString("F1")}초";
        m_crafting_progress_image.fillAmount = 1f;
        m_crafting_button.GetComponent<Image>().sprite = recipe.ButtonSprite;

        for(int i = m_recipe_content_transform.childCount; i <= recipe.RequireItems.Length; i++)
        {
            Instantiate(m_result_item_slot, Vector3.zero, Quaternion.identity, m_recipe_content_transform);
        }

        for(int i = 0; i < m_recipe_content_transform.childCount; i++)
        {
            var recipe_slot = m_recipe_content_transform.GetChild(i).GetComponent<InventorySlot>();

            if(i < recipe.RequireItems.Length)
            {
                recipe_slot.ClearSlot();
                m_main_inventory.LoadItem(recipe.RequireItems[i].Item, recipe_slot, recipe.RequireItems[i].Count);
                recipe_slot.gameObject.SetActive(true);
            }
            else
            {
                recipe_slot.gameObject.SetActive(false);
            }
        }
    }

    public void ToggleSlotState(bool is_craftable)
    {
        m_disable_image_object.SetActive(!is_craftable);
        m_crafting_button.interactable = is_craftable;
    }

    private void RefreshItems()
    {
        InventorySlot main_inventory_slot;

        foreach(var info in CurrentRecipe.RequireItems)
        {
            m_main_inventory.HasItemInInventory(info.Item.ID, out main_inventory_slot, info.Count);
            main_inventory_slot.UpdateSlotCount(-info.Count);
        }

        m_main_inventory.AcquireItem(CurrentRecipe.ResultItem.Item, CurrentRecipe.ResultItem.Count);

        //CraftManager.Instance.RefreshAllSlots();
    }

    private IEnumerator CoroutineCraftItem()
    {
        m_is_crafting = true;

        // TODO: 제작 사운드 추가

        float process = 0f;
        while(process < 1f)
        {
            process += Time.deltaTime / CurrentRecipe.CraftingTime;

            m_crafting_progress_image.fillAmount = Mathf.Lerp(0f, 1f, process);

            yield return null;
        }

        RefreshItems();
        m_is_crafting = false;
    }

    public void BTN_Craft()
    {
        if(m_is_crafting)
        {
            // TODO: 제작 중임을 알리는 텍스트 라벨
            return;
        }

        if(m_coroutine_craft is not null)
        {
            StopCoroutine(m_coroutine_craft);
        }
        m_coroutine_craft = StartCoroutine(CoroutineCraftItem());
    }
}
