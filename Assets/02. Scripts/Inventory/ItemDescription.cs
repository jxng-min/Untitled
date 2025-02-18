using System.Text;
using TMPro;
using UnityEngine;

public class ItemDescription : MonoBehaviour
{
    [Header("텍스트 관련")]
    [SerializeField] private GameObject m_tool_tip_object;
    [SerializeField] private Canvas m_canvas;

    [Header("매니저")]
    [SerializeField] private ItemDataManager m_item_data_manager;

    private TMP_Text m_text_label;
    private RectTransform m_rect_transform;
    private StringBuilder m_string_builder;

    private void Start()
    {
        m_text_label = m_tool_tip_object.GetComponentInChildren<TMP_Text>();
        m_rect_transform = m_canvas.GetComponent<RectTransform>();
        m_string_builder = new StringBuilder();

        m_tool_tip_object.SetActive(false);
    }

    public void Update()
    {
        if(m_tool_tip_object.activeInHierarchy)
        {
            CalculateMousePosition();
        }
    }

    public void OpenUI(int id)
    {
        m_string_builder.Clear();

        m_string_builder.Append("<b>");
        m_string_builder.AppendLine(m_item_data_manager.GetName(id));
        m_string_builder.Append("</b>");

        m_string_builder.AppendLine();
        m_string_builder.AppendLine(m_item_data_manager.GetDescription(id));

        m_text_label.SetText(m_string_builder.ToString());

        m_tool_tip_object.SetActive(true);

        m_tool_tip_object.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void CloseUI()
    {
        m_tool_tip_object.SetActive(false);
    }

    private void CalculateMousePosition()
    {
        Vector2 local_position;
        Vector2 mouse_position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var rect_transform = m_tool_tip_object.transform as RectTransform;

        Camera ui_camera = m_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_rect_transform, 
            mouse_position, 
            ui_camera, 
            out local_position
        );

        if(mouse_position.x < Screen.width * 0.15f)
        {
            local_position.x += rect_transform.sizeDelta.x; 
        }

        if(mouse_position.y > Screen.height * 0.8f)
        {
            local_position.y -= rect_transform.sizeDelta.y;
        }

        rect_transform.anchoredPosition = local_position;
    }
}
