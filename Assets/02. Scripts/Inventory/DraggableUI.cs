using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform m_rect_transform;
    private Canvas m_canvas;
    private Vector2 m_offset;


    private void Awake()
    {
        m_rect_transform = GetComponent<RectTransform>();
        m_canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_rect_transform.SetAsLastSibling();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_rect_transform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out m_offset
        );
    }

public void OnDrag(PointerEventData eventData)
{
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
        m_canvas.transform as RectTransform, 
        eventData.position, 
        eventData.pressEventCamera, 
        out Vector2 local_point))
    {
        Vector2 newPosition = local_point - m_offset;

        // 부모 Canvas의 RectTransform 가져오기
        RectTransform canvasRect = m_canvas.transform as RectTransform;

        // 현재 UI 요소의 크기
        Vector2 uiSize = m_rect_transform.rect.size * m_rect_transform.lossyScale;

        // 경계 설정
        float minX = -canvasRect.rect.width / 2 + uiSize.x / 2;
        float maxX = canvasRect.rect.width / 2 - uiSize.x / 2;
        float minY = -canvasRect.rect.height / 2 + uiSize.y / 2;
        float maxY = canvasRect.rect.height / 2 - uiSize.y / 2;

        // 위치 클램핑
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        m_rect_transform.localPosition = newPosition;
    }
}

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}