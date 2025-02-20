using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("상자의 이름")]
    [SerializeField] private string m_name;
    public string Name
    {
        get { return m_name; }
    }

    [Header("상자의 고유한 ID")]
    [SerializeField] private int m_id;
    public int ID
    {
        get { return m_id; }
    }

    [Header("상자에서 획득할 수 있는 아이템 목록")]
    [SerializeField] private Item[] m_item_list;

    [Header("상자에서 획득할 수 있는 아이템의 확률 목록(61 ~ 100까지 나눠서 할당)")]
    [SerializeField] private int[] m_item_probability;

    [Header("상자에서 등장할 수 있는 아이템의 최대 개수")]
    [SerializeField] private int m_appear_max_count;

    private ChestSlotInfo[] m_slot_infos = new ChestSlotInfo[9];
    public ChestSlotInfo[] SlotInfos
    {
        get { return m_slot_infos; }
    }

    public void Init()
    {
        for(int i = 0; i < 9; i++)
        {
            var probability = Random.Range(0, 101);

            if(probability < 61)
            {
                m_slot_infos[i] = new ChestSlotInfo((int)ItemCode.NONE, 0);

                continue;
            }

            for(int j = 0; j < m_item_probability.Length; j++)
            {
                if(probability < m_item_probability[j])
                {
                    if(m_item_list[j].Overlap)
                    {
                        m_slot_infos[i] = new ChestSlotInfo(m_item_list[j].ID, Random.Range(1, m_appear_max_count));
                    }
                    else
                    {
                        m_slot_infos[i] = new ChestSlotInfo(m_item_list[j].ID, 1);
                    }

                    break;
                }
            }
        }
    }

    public void LoadData(ChestInfo chest_info)
    {
        for(int i = 0; i < m_slot_infos.Length; i++)
        {
            m_slot_infos[i] = chest_info.SlotInfos[i];
        }
    }

    public ChestInfo SaveData()
    {
        return new ChestInfo(m_id, m_slot_infos);
    }

    public void TryOpenChestUI()
    {
        ChestDataManager.Instance.TryOpenChestUI(ID);
    }
}
