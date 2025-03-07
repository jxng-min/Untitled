using UnityEngine;
using TMPro;
using UnityEditor;

public class NPCRaycast : MonoBehaviour
{
    private RaycastHit m_hit;
    private bool m_is_interaction_active = false;
    private NPCInteraction m_current_npc;

    [Header("레이어 마스크")]
    [SerializeField] private LayerMask m_layer_mask;

    [Header("레이 캐스팅 거리")]
    [SerializeField] private float m_ray_distance;

    [Header("레이를 발사하는 카메라")]
    [SerializeField] private Camera m_ray_camera;

    [Header("아이템 인디케이터")]
    [SerializeField] private GameObject m_indicator;
    [SerializeField] private TMP_Text m_indicator_label;

    [Header("추가/삭제할 머티리얼")]
    [SerializeField] private Material m_material;
    private bool m_material_exist = false;

    private void Update()
    {
        CheckNPC();
        m_indicator.transform.LookAt(m_ray_camera.transform);

        if(m_is_interaction_active)
        {
            TryInteraction();
        }   
    }

    private void CheckNPC()
    {
        if(Physics.Raycast(m_ray_camera.transform.position, m_ray_camera.transform.forward, out m_hit, m_ray_distance, m_layer_mask))
        {
            if(m_hit.collider.CompareTag("NPC"))
            {
                NPCInteraction raycasted_npc = m_hit.transform.GetComponent<NPCInteraction>();

                if(m_current_npc == raycasted_npc)
                {
                    return;
                }

                if(m_current_npc is not null)
                {
                    RemoveMaterial();
                }

                m_current_npc = raycasted_npc;

                m_indicator.SetActive(true);
                m_indicator_label.text = NPCDataManager.Instance.GetName(m_current_npc.Info.ID);

                Vector3 final_position = m_hit.transform.position + new Vector3(0f, m_current_npc.Indicator, 0f);
                m_indicator.transform.position = final_position;

                m_is_interaction_active = true;

                AddMaterial();

                return;
            }
            else
            {
                NPCInfoDisappear();
            }
        }
        else
        {
            if(m_current_npc is not null)
            {
                RemoveMaterial();
            }
            NPCInfoDisappear();
        }
    }

    private void TryInteraction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(m_current_npc is null)
            {
                return;
            }

            if(m_current_npc.Info.Type == NPCType.NONE)
            {
                if(m_current_npc.Info.Interaction)
                {
                    switch(m_current_npc.name)
                    {
                        case "Camp Leader_NPC":
                            if(m_current_npc.GetComponent<QuestNPC>().IsExistQuest(out int quest_id1))
                            {
                                switch(quest_id1)
                                {
                                    case 0:
                                        CheckQuest(quest_id1, 2, 3, 5, 1, 6, 1);
                                        break;

                                    case 1:
                                        CheckQuest(quest_id1, 7, 2, 9, 1, 6, 1);
                                        break;

                                    case 3:
                                        CheckQuest(quest_id1, 10, 3, 13, 1, 14, 1);
                                        break;
                                    
                                    case 4:
                                        CheckQuest(quest_id1, 15, 2, 1, 1, 17, 1);
                                        break;
                                    
                                    case 6:
                                        CheckQuest(quest_id1, 18, 3, 21, 1, 22, 1);
                                        break;
                                    
                                    case 9:
                                        CheckQuest(quest_id1, 23, 2, 1, 1, 24, 1);
                                        break;
                                    
                                    case 10:
                                        CheckQuest(quest_id1, 26, 5, 31, 1, 32, 4);
                                        break;
                                }
                                
                            }
                            else
                            {
                                ConversationManager.Instance.Dialoging(m_current_npc.Info.ID, 0, 2);
                            }
                            break;

                        case "Weary Wanderer_NPC":
                            if(m_current_npc.GetComponent<QuestNPC>().IsExistQuest(out int quest_id2))
                            {
                                switch(quest_id2)
                                {
                                    case 2:
                                        CheckQuest(quest_id2, 1, 2, 3, 1, 4, 2);
                                        break;
                                    
                                    case 7:
                                        CheckQuest(quest_id2, 6, 2, 8, 1, 9, 2);
                                        break;
                                }
                            }
                            else
                            {
                                ConversationManager.Instance.Dialoging(m_current_npc.Info.ID, 0, 1);
                            }
                            break;

                        case "Suspicious_NPC":
                            if(m_current_npc.GetComponent<QuestNPC>().IsExistQuest(out int quest_id3))
                            {
                                switch(quest_id3)
                                {
                                    case 5:
                                        CheckQuest(quest_id3, 1, 3, 4, 1, 5, 5);
                                        break;
                                }
                            }
                            else
                            {
                                ConversationManager.Instance.Dialoging(m_current_npc.Info.ID, 0, 1);
                            }
                            break;

                        case "Guard_NPC":
                            if(m_current_npc.GetComponent<QuestNPC>().IsExistQuest(out int quest_id4))
                            {
                                switch(quest_id4)
                                {
                                    case 8:
                                        CheckQuest(quest_id4, 1, 3, 4, 1, 5, 1);
                                        break;
                                }
                            }
                            else
                            {
                                ConversationManager.Instance.Dialoging(m_current_npc.Info.ID, 0, 1);
                            }
                            break;
                    }
                }
            }
            else
            {
                switch(m_current_npc.Info.Type)
                {
                    case NPCType.MERCHANT:
                        if(!ItemShopManager.IsActive)
                        {
                            ConversationManager.Instance.Dialoging(m_current_npc.Info.ID);

                            if(!ConversationManager.Instance.IsTalking)
                            {
                                SoundManager.Instance.PlayBGM("Shop Background");
                                m_current_npc.GetComponent<Merchant>().Trade();
                            }
                        }
                        break;
                    
                    case NPCType.REINFORCER:
                        break;

                    case NPCType.CRAFTSMAN:
                        if(!CraftingManager.IsActive)
                        {
                            ConversationManager.Instance.Dialoging(m_current_npc.Info.ID);

                            if(!ConversationManager.Instance.IsTalking)
                            {
                                SoundManager.Instance.PlayBGM("Craft Station Background");
                                m_current_npc.GetComponent<CraftingStation>().TryOpenDialog();
                            }
                        }
                        break;
                    
                    case NPCType.CHEST:
                        if(!ChestDataManager.IsActive)
                        {
                            m_current_npc.GetComponent<Chest>().TryOpenChestUI();
                        }
                        break;
                }
            }

            if(ConversationManager.Instance.IsTalking)
            {
                if(ConversationManager.Instance.TalkIndex == 1)
                {
                    ConversationManager.Instance.BubbleDialoging(m_hit.transform, m_current_npc, m_hit.normal);
                }
            }
        }
    }

    private void NPCInfoDisappear()
    {
        m_indicator_label.text = "";
        m_indicator.SetActive(false);

        m_current_npc = null;
    }

    private void CheckQuest(int quest_id, int never_index, int never_count, int going_index, int going_count, int clear_index, int clear_count)
    {
        if(quest_id == -1)
        {
            return;
        }

        if(QuestManager.Instance.CheckQuestState(quest_id) == QuestState.NEVER_RECEIVED)
        {
            ConversationManager.Instance.Dialoging(m_current_npc.Info.ID, never_index, never_count);

            if(!ConversationManager.Instance.IsTalking)
            {
                QuestManager.Instance.ReceiveQuest(quest_id);
            }
        }
        else if(QuestManager.Instance.CheckQuestState(quest_id) == QuestState.ON_GOING)
        {
            ConversationManager.Instance.Dialoging(m_current_npc.Info.ID, going_index, going_count);
        }

        else if(QuestManager.Instance.CheckQuestState(quest_id) == QuestState.CLEAR)
        {
            ConversationManager.Instance.Dialoging(m_current_npc.Info.ID, clear_index, clear_count);

            if(!ConversationManager.Instance.IsTalking)
            {
                QuestManager.Instance.CompleteQuest(quest_id);
            }
        }
    }

    private void AddMaterial()
    {
        foreach(var renderer in m_current_npc.GetComponentsInChildren<Renderer>(true))
        {
            Material[] materials = renderer.sharedMaterials;
            m_material_exist = false;

            foreach(var material in materials)
            {
                if(material == m_material)
                {
                    m_material_exist = true;
                    break;
                }
            }

            if(!m_material_exist)
            {
                ArrayUtility.Add(ref materials, m_material);
                renderer.sharedMaterials = materials;
            }
        }
    }

    private void RemoveMaterial()
    {
        foreach(var renderer in m_current_npc.GetComponentsInChildren<Renderer>(true))
        {
            Material[] materials = renderer.sharedMaterials;

            for(int i = 0; i < materials.Length; i++)
            {
                if(materials[i] == m_material)
                {
                    ArrayUtility.RemoveAt(ref materials, i);
                    renderer.sharedMaterials = materials;

                    break;
                }
            }
        }
    }
}
