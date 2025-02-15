using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DialogueInfo
{
    public int m_npc_id;
    public string[] m_npc_dialogue;
}

[System.Serializable]
public class DialogueInfoList
{
    public DialogueInfo[] m_dialogue_infos;
}

public class ConversationManager : Singleton<ConversationManager>
{
    private string m_dialogue_data_path;
    private Dictionary<int, string[]> m_dialogue_data;

    private int m_current_talk_index;

    private bool m_is_talking = false;
    public bool IsTalking
    {
        get { return m_is_talking; }
    }

    [Header("대화창 오브젝트")]
    [SerializeField] private GameObject m_dialogue_object;

    [Header("대화창 NPC 이름")]
    [SerializeField] private TMP_Text m_dialogue_name_label;

    [Header("대화창 NPC 대화")]
    [SerializeField] private TypeEffect m_dialogue_text_label;

    private void Start()
    {
        m_dialogue_data_path = Path.Combine(Application.persistentDataPath, "DialogueData.json");
        m_dialogue_data = new Dictionary<int, string[]>();

        ParsingJson();
    }

    private void ParsingJson()
    {
        if(File.Exists(m_dialogue_data_path))
        {
            var json_data = File.ReadAllText(m_dialogue_data_path);
            var dialogue_list = JsonUtility.FromJson<DialogueInfoList>(json_data);

            if(dialogue_list is not null && dialogue_list.m_dialogue_infos is not null)
            {
                foreach(var diaglogue in dialogue_list.m_dialogue_infos)
                {
                    m_dialogue_data.Add(diaglogue.m_npc_id, diaglogue.m_npc_dialogue);
                }
            }
            else
            {
                Debug.Log("Json format is incorrect or empty.");
            }
        }
        else
        {
            Debug.Log($"{m_dialogue_data_path} is not existed.");
        }        
    }

    private string GetDialogue(int npc_id, int dialogue_index)
    {
        if(dialogue_index == m_dialogue_data[npc_id].Length)
        {
            return null;
        }
        else
        {
            return m_dialogue_data.ContainsKey(npc_id) ? m_dialogue_data[npc_id][dialogue_index] : null;
        }
    }

    public void Dialoging(int npc_id)
    {
        var dialogue = GetDialogue(npc_id, m_current_talk_index);

        if(dialogue is null)
        {
            m_is_talking = false;
            ToggleDialogue(m_is_talking);
            m_current_talk_index = 0;

            return;
        }

        m_is_talking = true;
        ToggleDialogue(m_is_talking);

        m_dialogue_name_label.text = NPCDataManager.Instance.GetName(npc_id);
        m_dialogue_text_label.SetDialogue(dialogue);
        
        m_current_talk_index++;
    }

    private void ToggleDialogue(bool flag)
    {
        m_dialogue_object.SetActive(flag);
        m_dialogue_name_label.gameObject.SetActive(flag);
        m_dialogue_text_label.gameObject.SetActive(flag);
    }
}
