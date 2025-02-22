using TMPro;
using UnityEngine;

public class QuestIndicator : MonoBehaviour
{
    [Header("인디케이터의 중심축")]
    [SerializeField] private Transform m_child_transform;

    [Header("인디케이터 텍스트 라벨")]
    [SerializeField] private TMP_Text m_indicator_text_label;

    private void Update()
    {
        m_child_transform.LookAt(Camera.main.transform);
    }

    public void ToggleWithUpdateIndicator(string text, bool is_active)
    {
        gameObject.SetActive(is_active);
        m_indicator_text_label.text = text;
    }
}
