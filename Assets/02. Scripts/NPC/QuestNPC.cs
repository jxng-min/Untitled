using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    [Header("이 NPC가 가질 퀘스트 목록")]
    [SerializeField] QuestData[] m_quest_data_list;

    [Header("퀘스트 인디케이터 오브젝트")]
    [SerializeField] QuestIndicator m_indicator_object;
    public QuestIndicator Indicator
    {
        get { return m_indicator_object; }
    }

    public bool IsExistQuest(out int quest_id)
    {
        quest_id = -1;

        foreach(var quest_data in m_quest_data_list)
        {
            if(QuestManager.Instance.CheckQuestState(quest_data.ID) == QuestState.CLEARED_PAST)
            {
                continue;
            }

            for(int i = 0; i < quest_data.PrerequisteQuestIDs.Length; i++)
            {
                if(QuestManager.Instance.CheckQuestState(quest_data.PrerequisteQuestIDs[i]) != QuestState.CLEARED_PAST)
                {
                    return false;
                }
            }

            if(quest_data.PrerequisteLevel > DataManager.Instance.Data.Level)
            {
                return false;
            }

            quest_id = quest_data.ID;
            return true;
        }

        return false;
    }

    public void Update()
    {
        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        int quest_id;

        if(IsExistQuest(out quest_id))
        {
            switch(QuestManager.Instance.CheckQuestState(quest_id))
            {
                case QuestState.NEVER_RECEIVED:
                    Indicator.ToggleWithUpdateIndicator("!", true);
                    break;

                case QuestState.ON_GOING:
                    Indicator.ToggleWithUpdateIndicator("...", true);
                    break;

                case QuestState.CLEAR:
                    Indicator.ToggleWithUpdateIndicator("?", true);
                    break;
                
                default:
                    Indicator.ToggleWithUpdateIndicator("", false);
                    break;
            }
        }
        else
        {
            Indicator.ToggleWithUpdateIndicator("", false);
        }
    }
}
