using Junyoung;
using UnityEngine;
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

        NavigationManager.Instance.TryStopNavigation(m_quest_data.name);
    }

    public void TOGGLE_CompactContent()
    {
        QuestUIManager.Instance.ToggleCompactQuestContent(m_quest_data, m_compact_content_toggle.isOn);
        SoundManager.Instance.PlayEffect("Button Click");
    }

    public void BTN_DisplayCurrentQuest()
    {
        QuestUIManager.Instance.ToggleFullQuestContent(m_quest_data);
        SoundManager.Instance.PlayEffect("Button Click");
    }

    public void BTN_Nav()
    {
        if(m_quest_data.name == NavigationManager.Instance.NavKeyName)
        {
            NavigationManager.Instance.TryStopNavigation(m_quest_data.name);
            return;
        }

        switch(QuestManager.Instance.CheckQuestState(m_quest_data.ID))
        {
            case QuestState.CLEAR:
                NavigationManager.Instance.StartNavigation(m_quest_data.name, GameManager.Instance.transform, m_quest_data.Source);
                break;
            
            case QuestState.ON_GOING:
                NavigationManager.Instance.StartNavigation(m_quest_data.name, GameManager.Instance.transform, m_quest_data.Destination);
                break;            
        }
        
        SoundManager.Instance.PlayEffect("Button Click");
    }
}
