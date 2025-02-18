using TMPro;
using UnityEngine;

public class TypeEffect : MonoBehaviour
{
    private string m_target_dialogue;
    private int m_index;
    private float m_interval;
    private bool m_is_effecting;
    private TMP_Text m_dialogue_text_label;

    [Header("1초 안에 출력될 문자의 개수")]
    [SerializeField] int m_character_per_sec;

    [Header("문장을 끝을 나타낼 엔드커서")]
    [SerializeField] GameObject m_end_cursor;

    private void Awake()
    {
        m_dialogue_text_label = GetComponent<TMP_Text>();   
    }

    public void SetDialogue(string dialogue)
    {
        if(m_is_effecting)
        {
            m_dialogue_text_label.text = m_target_dialogue;

            CancelInvoke();
            EndEffect();
        }

        m_target_dialogue = dialogue;

        BeginEffect();
    }

    private void BeginEffect()
    {
        m_dialogue_text_label.text = "";
        m_index = 0;
        m_is_effecting = true;

        m_end_cursor.SetActive(false);

        m_interval = 1f / m_character_per_sec;
        Invoke("Effecting", m_interval);
    }

    private void Effecting()
    {
        if(m_dialogue_text_label.text == m_target_dialogue)
        {
            EndEffect();
            return;
        }

        m_dialogue_text_label.text += m_target_dialogue[m_index];
        m_index++;

        Invoke("Effecting", m_interval);
    }

    private void EndEffect()
    {
        m_is_effecting = false;
        m_end_cursor.SetActive(true);
    }
}
