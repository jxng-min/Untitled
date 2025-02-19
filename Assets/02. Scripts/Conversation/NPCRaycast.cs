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
                    ConversationManager.Instance.Dialoging(m_current_npc.Info.ID);
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
                                m_current_npc.GetComponent<Merchant>().Trade();
                            }
                        }
                        break;
                    
                    case NPCType.REINFORCER:
                        break;

                    case NPCType.CRAFTSMAN:
                        ConversationManager.Instance.Dialoging(m_current_npc.Info.ID);
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
