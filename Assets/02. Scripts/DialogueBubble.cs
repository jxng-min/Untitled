using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    [Header("말풍선의 중심축")]
    [SerializeField] private Transform m_child_transform;

    [Header("말풍선 이미지")]
    [SerializeField] private GameObject m_bubble_image;

    [Header("말풍선 NPC 대화")]
    [SerializeField] private TypeEffect m_bubble_text_label;

    private Coroutine m_coroutine_fade_label;

    private void Update()
    {
        m_child_transform.LookAt(Camera.main.transform);
    }

    public void Init(Vector3 pos, int npc_id, float size = 1f)
    {
        transform.localScale = Vector3.one * size;
        transform.position = pos;

        m_bubble_text_label.SetDialogue(ConversationManager.Instance.GetBubbleData(npc_id));

        if(m_coroutine_fade_label is not null)
        {
            StopCoroutine(m_coroutine_fade_label);
        }
        m_coroutine_fade_label = StartCoroutine(DisableBubble());
    }
    private IEnumerator DisableBubble()
    {
        yield return new WaitForSeconds(2f);

        float elapsed_time = 0f;
        float target_time = 1f;

        //Image bubble_image = m_bubble_image.GetComponent<Image>();
        TMP_Text bubble_text = m_bubble_text_label.GetComponent<TMP_Text>();

        //Color m_image_color = bubble_image.color;
        Color m_text_color = bubble_text.color;

        //float start_image_alpha = bubble_image.color.a;
        float start_text_alpha = bubble_text.color.a;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            float t = elapsed_time / target_time;

            //bubble_image.color = new Color(m_image_color.r, m_image_color.g, m_image_color.b, Mathf.Lerp(start_image_alpha, 0f, t));
            bubble_text.color = new Color(m_text_color.r, m_text_color.g, m_text_color.b, Mathf.Lerp(start_text_alpha, 0f, t));

            yield return null;
        }

        //bubble_image.color = new Color(m_image_color.r, m_image_color.g, m_image_color.b, 0f);
        bubble_text.color = new Color(m_text_color.r, m_text_color.g, m_text_color.b, 0f);

        gameObject.SetActive(false);
    }
}
