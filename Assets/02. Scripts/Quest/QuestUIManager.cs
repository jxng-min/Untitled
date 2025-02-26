using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : Singleton<QuestUIManager>
{
    private static bool m_is_ui_active = false;
    public static bool IsActive
    {
        get { return m_is_ui_active; }
    }

    [Header("전체 사이즈 퀘스트 UI 오브젝트")]
    [SerializeField] private GameObject m_full_size_quest_ui_object;

    private Dictionary<int, QuestContentData> m_quest_contents = new Dictionary<int, QuestContentData>();

    [Space(50)]
    [Header("UI 관련")]
    
    [Header("컴팩트 퀘스트 UI를 인스턴스화 할 부모 트랜스폼")]
    [SerializeField] private RectTransform m_quest_compact_root;

    [Header("컴팩트 퀘스트 UI 프리팹")]
    [SerializeField] private GameObject m_quest_compact_prefab;

    [Header("전체 사이즈 퀘스트 UI를 인스턴스화 할 부모 프랜스폼")]
    [SerializeField] private RectTransform m_quest_full_root;

    [Header("전체 사이즈 퀘스트 UI 프리팹")]
    [SerializeField] private GameObject m_quest_full_prefab;

    [Header("전체 사이즈 선택 컨텐츠 라벨")]
    [SerializeField] private TMP_Text m_full_selected_content_label;

    private Dictionary<int, QuestCompactContent> m_compact_quest_contents = new Dictionary<int, QuestCompactContent>();
    private Dictionary<int, QuestFullContent> m_full_quest_contents = new Dictionary<int, QuestFullContent>();

    private new void Awake()
    {
        base.Awake();

        m_is_ui_active = false;

        foreach(QuestContentData content_data in QuestManager.Instance.QuestContentReader.DataList)
        {
            m_quest_contents.Add(content_data.m_quest_id, content_data);
        }        
    }

    private void Update()
    {
        TryOpenQuestUI();   
    }

    private void TryOpenQuestUI()
    {
        if(!SettingManager.IsActive)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                if(m_full_size_quest_ui_object.activeInHierarchy)
                {
                    m_full_size_quest_ui_object.SetActive(false);
                    m_is_ui_active = false;

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    m_full_size_quest_ui_object.SetActive(true);
                    m_is_ui_active = true;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
    }

    public void CompleteQuest(QuestData quest_data)
    {
        ToggleCompactQuestContent(quest_data, false);
        m_full_quest_contents[quest_data.ID].CompleteQuest();
    }

    public void ToggleCompactQuestContent(QuestData quest_data, bool is_enable)
    {
        if(is_enable)
        {
            if(m_compact_quest_contents.ContainsKey(quest_data.ID))
            {
                Debug.LogErrorFormat(
                    "{0} 퀘스트가 중복됩니다."
                    , quest_data.ID
                );
            }
            else
            {
                QuestCompactContent new_quest_content = Instantiate(m_quest_compact_prefab, Vector3.zero, Quaternion.identity, m_quest_compact_root).GetComponent<QuestCompactContent>();

                new_quest_content.Init(quest_data);

                m_compact_quest_contents.Add(quest_data.ID, new_quest_content);

                new_quest_content.UpdateCompactQuestContents(m_quest_contents[quest_data.ID]);
            }
        }
        else
        {
            if(m_compact_quest_contents.ContainsKey(quest_data.ID))
            {
                Destroy(m_compact_quest_contents[quest_data.ID].gameObject);
                m_compact_quest_contents.Remove(quest_data.ID);
            }
        }

        StartCoroutine(RefreshQuestCompactLayout());
    }

    public void AddFullQuestContent(QuestData quest_data)
    {
        if(m_full_quest_contents.ContainsKey(quest_data.ID))
        {
            Debug.LogErrorFormat(
                "{0} 퀘스트가 중복됩니다."
                , quest_data.ID
            );
        }
        else
        {
            QuestFullContent new_quest_content = Instantiate(m_quest_full_prefab, Vector3.zero, Quaternion.identity, m_quest_full_root).GetComponent<QuestFullContent>();

            new_quest_content.Init(quest_data);

            m_full_quest_contents.Add(quest_data.ID, new_quest_content);

            new_quest_content.UpdateCompactQuestContents(m_quest_contents[quest_data.ID]);

            new_quest_content.transform.SetAsFirstSibling();

            ToggleCompactQuestContent(quest_data, true);
        }
    }

    public void UpdateCurrentQuestState(int quest_id)
    {
        if(m_compact_quest_contents.ContainsKey(quest_id))
        {
            m_compact_quest_contents[quest_id].UpdateCompactQuestContents(m_quest_contents[quest_id]);
        }

        if(m_full_quest_contents.ContainsKey(quest_id))
        {
            m_full_quest_contents[quest_id].UpdateCompactQuestContents(m_quest_contents[quest_id]);
        }
    }

    public void ToggleFullQuestContent(QuestData quest_data, bool is_enable = true)
    {
        if(is_enable is false)
        {
            m_full_selected_content_label.text = "";
        }
        else
        {
            QuestContentData content_data = m_quest_contents[quest_data.ID];

            StringBuilder string_builder = new StringBuilder();

            string_builder.Append($"<size=20>{content_data.m_title}\n\n</size>");
            string_builder.Append($"<size=15>의뢰인: {content_data.m_receive_from}\n</size>");
            string_builder.Append($"\n<size=15>{content_data.m_full_content}</size>");

            m_full_selected_content_label.text = string_builder.ToString();
        }
    }

    private IEnumerator RefreshQuestCompactLayout()
    {
        var compact_root = m_quest_compact_root.GetComponent<VerticalLayoutGroup>();

        compact_root.reverseArrangement = true;
        
        yield return null;

        compact_root.reverseArrangement = false;
    }
}
