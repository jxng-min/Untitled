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
            out Vector2 localPoint))
        {
            m_rect_transform.localPosition = localPoint - m_offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}