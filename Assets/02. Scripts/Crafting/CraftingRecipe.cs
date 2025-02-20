#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Scriptable Object/Create Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("제작에 필요한 재료 아이템들")]
    [SerializeField] private CraftingItemInfo[] m_require_items;
    public CraftingItemInfo[] RequireItems
    {
        get { return m_require_items; }
    }

    [Header("제작할 대상 아이템")]
    [SerializeField] private CraftingItemInfo m_result_item;
    public CraftingItemInfo ResultItem
    {
        get { return m_result_item; }
    }

    [Space(30)]
    [Header("요구되는 레벨")]
    [SerializeField] private int m_require_level;
    public int RequireLevel
    {
        get { return m_require_level; }
    }

    [Header("제작에 걸리는 시간")]
    [SerializeField] private float m_crafting_time;
    public float CraftingTime
    {
        get { return m_crafting_time; }
    }

    [Header("아이콘을 표시할 이미지")]
    [SerializeField] private Sprite m_button_sprite;
    public Sprite ButtonSprite
    {
        get { return m_button_sprite; }
    }
}

[System.Serializable]
public class CraftingItemInfo
{
    [SerializeField] private Item m_item;
    public Item Item
    {
        get { return m_item; }
    }

    [SerializeField] private int m_count;
    public int Count
    {
        get { return m_count; }
    }
}