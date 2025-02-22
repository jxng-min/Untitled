using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestFullContent : QuestCompactContent
{
    [SerializeField] private Toggle m_compact_content_toggle;
    [SerializeField] private Button m_nav_button;

    public override void UpdateCompactQuestContents(QuestContentData content_data)
    {
        base.UpdateCompactQuestContents(content_data);
    }

    public void CompleteQuest()
    {
        Destroy(m_compact_content_toggle.gameObject);
        Destroy(m_nav_button.gameObject);
    }

    public void TOGGLE_CompactContent()
    {
        QuestUIManager.Instance.ToggleCompactQuestContent(m_quest_data, m_compact_content_toggle.isOn);
    }

    public void BTN_DisplayCurrentQuest()
    {
        QuestUIManager.Instance.ToggleFullQuestContent(m_quest_data);
    }

    public void BTN_Nav()
    {
        if(SceneManager.GetActiveScene().name != m_quest_data.Scene)
        {
            return;
        }

        // TODO: 길 안내 시스템 완성
    }
}
