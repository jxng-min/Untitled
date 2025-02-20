using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    [Header("제작소에서 제작할 수 있는 레시피 목록")]
    [SerializeField] private CraftingRecipe[] m_recipes;

    [Header("글로버 레시피의 유무")]
    [SerializeField] private bool m_is_use_global_recipes = true;

    [Header("제작소 이름")]
    [SerializeField] private string m_title = "제작소";

    public void TryOpenDialog()
    {
        CraftingManager.Instance.TryOpenCraftingUI(m_recipes, m_is_use_global_recipes, m_title);
    }
}
