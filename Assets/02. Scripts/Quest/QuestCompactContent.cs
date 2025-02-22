using System.Text;
using TMPro;
using UnityEngine;

public class QuestCompactContent : MonoBehaviour
{
    protected QuestData m_quest_data;

    [Header("퀘스트 제목 라벨")]
    [SerializeField] private TMP_Text m_title_text_label;

    [Header("퀘스트 내용 라벨")]
    [SerializeField] private TMP_Text m_content_text_label;

    public void Init(QuestData quest_data)
    {
        m_quest_data = quest_data;
    }

    public virtual void UpdateCompactQuestContents(QuestContentData content_data)
    {
        string compact_title_text
            = content_data.m_title + " " + ((QuestManager.Instance.CheckQuestState(m_quest_data.ID) == QuestState.CLEAR || QuestManager.Instance.CheckQuestState(m_quest_data.ID) == QuestState.CLEARED_PAST)
            ? "<color=green>(완료)</color>" : "<color=red>(미완료)</color>");

        m_title_text_label.text = compact_title_text;

        StringBuilder content_text = new StringBuilder();
        for(int i = 0; i < content_data.m_compact_content.Length; i++)
        {
            if(content_data.m_compact_content[i] == '{')
            {
                int format_from = i;

                for(; i < content_data.m_compact_content.Length; i++)
                {
                    if(content_data.m_compact_content[i] == '}')
                    {
                        int format_index = int.Parse(content_data.m_compact_content.Substring(format_from + 1, i - format_from - 1));

                        content_text.Append(GetFormatIndex(m_quest_data, format_index));
                        break;
                    }
                }
            }
            else
            {
                content_text.Append(content_data.m_compact_content[i]);
            }
        }
        
        m_content_text_label.text = content_text.ToString();
    }

    private string GetFormatIndex(QuestData quest_data, int format_id)
    {
        for(int i = 0; i < quest_data.All.Length; i++)
        {
            if(quest_data.All[i].ID == format_id)
            {
                return quest_data.All[i].GetProgressText();
            }
        }

        Debug.LogErrorFormat(
            "{0}에서 {1}번 퀘스트 포맷 인덱스가 없습니다."
            , quest_data.name
            , format_id
        );

        return null;
    }
}
