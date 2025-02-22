using UnityEngine;
using System.Collections.Generic;
using Junyoung;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuestManager : Singleton<QuestManager>
{
    [Header("메인 인벤토리")]
    [SerializeField] private InventoryMain m_main_inventory;

    [Header("에디터에서 로드한 퀘스트 목록")]
    [SerializeField] private List<QuestData> m_preload_quests;

    [Header("현재까지 로드된 총 퀘스트의 개수(절대 수정 X)")]
    [field: SerializeField] public int QuestCount { get; private set; } = -1;

    private Dictionary<int, QuestData> m_quests = new Dictionary<int, QuestData>();
    public Dictionary<int, QuestData> Quests
    {
        get { return m_quests; }
    }

    private List<QuestData> m_received_quests = new List<QuestData>();
    public List<QuestData> ReceivedQuests
    {
        get { return m_received_quests; }
    }

    private string m_all_quest_data_path;
    private QuestContentDataList m_quest_content_list;
    public QuestContentDataList QuestContentReader
    {
        get { return m_quest_content_list; }
        private set { m_quest_content_list = value; }
    }
    

    private new void Awake()
    {
        base.Awake();

        m_all_quest_data_path = Path.Combine(Application.persistentDataPath, "AllQuestData.json");
        LoadData();

        LoadAllQuests();
    }

    private void LoadData()
    {
        if(File.Exists(m_all_quest_data_path))
        {
            var json_data = File.ReadAllText(m_all_quest_data_path);
            QuestContentReader = JsonUtility.FromJson<QuestContentDataList>(json_data); 
        }
        else
        {
            Debug.Log($"{m_all_quest_data_path}가 존재하지 않습니다.");
        }
    }

    private void LoadAllQuests()
    {
        foreach(QuestData quest_data in m_preload_quests)
        {
            var new_quest = quest_data;

            List<QuestBase> all_quests = new List<QuestBase>();

            foreach(var kill_quest in new_quest.KillQuests)
            {
                all_quests.Add(kill_quest);
            }

            foreach(var item_quest in new_quest.ItemQuests)
            {
                all_quests.Add(item_quest);
            }

            new_quest.All = all_quests.ToArray();
            
            Quests.Add(quest_data.ID, new_quest);
        }
    }

    public void ReceiveQuest(int quest_id)
    {
        QuestData quest_data = Quests[quest_id];

        QuestUIManager.Instance.AddFullQuestContent(quest_data);

        m_received_quests.Add(quest_data);

        if(quest_data.QuestState == QuestState.CLEARED_PAST)
        {
            CompleteQuest(quest_data.ID, false);
        }
        else
        {
            quest_data.QuestState = QuestState.ON_GOING;
        }

        QuestUIManager.Instance.UpdateCurrentQuestState(quest_data.ID);
        UpdateItemQuestCount();
    }

    public void CompleteQuest(int quest_id, bool is_give_reward = true)
    {
        QuestData quest_data = Quests[quest_id];

        if(is_give_reward)
        {
            DataManager.Instance.Data.EXP += quest_data.EXP;
            
            for(int i = 0; i < quest_data.Items.Length; i++)
            {
                m_main_inventory.AcquireItem(quest_data.Items[i], quest_data.ItemCounts[i]);
            }

            UpdateItemQuestCount();
        }

        QuestUIManager.Instance.CompleteQuest(quest_data);

        m_received_quests.Remove(quest_data);

        quest_data.QuestState = QuestState.CLEARED_PAST;
    }

    public QuestState CheckQuestState(int quest_id)
    {
        if(Quests[quest_id].QuestState == QuestState.CLEARED_PAST)
        {
            return QuestState.CLEARED_PAST;
        }

        foreach(QuestData quest_data in m_received_quests)
        {
            if(quest_data.ID == quest_id)
            {
                foreach(QuestBase quest in quest_data.All)
                {
                    if(quest.IsParticularClear is false)
                    {
                        return QuestState.ON_GOING;
                    }
                }

                return QuestState.CLEAR;
            }
        }

        return QuestState.NEVER_RECEIVED;
    }

    public void UpdateKillQuestCount(EnemyType enemy_code)
    {
        for(int i = 0; i < m_received_quests.Count; i++)
        {
            for(int j = 0; j < m_received_quests[i].KillQuests.Length; j++)
            {
                if(m_received_quests[i].KillQuests[j].EnemyType == enemy_code)
                {
                    ++m_received_quests[i].KillQuests[j].CurrentCount;

                    m_received_quests[i].KillQuests[j].IsParticularClear
                        = m_received_quests[i].KillQuests[j].CurrentCount >= m_received_quests[i].KillQuests[j].TotalCount;
                    
                    QuestUIManager.Instance.UpdateCurrentQuestState(m_received_quests[i].ID);

                    return;
                }
            }
        }
    }

    public void UpdateItemQuestCount()
    {
        foreach(QuestData quest_data in m_received_quests)
        {
            foreach(ItemQuest item_quest in quest_data.ItemQuests)
            {
                item_quest.CurrentCount = m_main_inventory.GetItemCount(item_quest.ItemCode);

                item_quest.IsParticularClear
                    = item_quest.CurrentCount >= item_quest.TotalCount;

                QuestUIManager.Instance.UpdateCurrentQuestState(quest_data.ID);
            }
        }
    }

    #region Editor Method
#if UNITY_EDITOR
    public void LoadQuests(List<QuestData> all_quests)
    {
        m_preload_quests = new List<QuestData>();
        m_preload_quests = all_quests;

        QuestCount = all_quests is null ? -1 : all_quests.Count;
    }
#endif
    #endregion
}

#region Editor Function
#if UNITY_EDITOR
[CustomEditor(typeof(QuestManager))]
public class QuestManager_EditorFunctions : Editor
{
    private QuestManager m_base_target;

    private void OnEnable()
    {
        m_base_target = (QuestManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n모든 퀘스트 불러오기");

        if(GUILayout.Button("불러오기"))
        {
            LoadToArray();
        }
    }

    private void LoadToArray()
    {
        bool is_duplicated = false;

        string[] guid_array = AssetDatabase.FindAssets("t:QuestData");

        List<QuestData> quests = new List<QuestData>();
        Dictionary<int, QuestData> quest_duplicate = new Dictionary<int, QuestData>();

        foreach(string guid in guid_array)
        {
            var asset_path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<QuestData>(asset_path);

            if(quest_duplicate.ContainsKey(asset.ID))
            {
                Debug.LogErrorFormat(
                    "{0}와 {1}가 퀘스트 ID {2}로 겹칩니다!"
                    , quest_duplicate[asset.ID].name
                    , asset.name
                    , asset.ID);
                
                is_duplicated = true;
                
                break;
            }

            quest_duplicate.Add(asset.ID, asset);
            quests.Add(asset);
        }

        if(!is_duplicated)
        {
            Debug.LogFormat(
                "<color=cyan>{0}개의 퀘스트가 중복 없이 로드되었습니다.</color>"
                , quest_duplicate.Count);
        }
        else
        {
            quest_duplicate.Clear();
            quest_duplicate = null;
            quests.Clear();
            quests = null;
        }

        m_base_target.LoadQuests(quests);
    }
}
#endif
#endregion