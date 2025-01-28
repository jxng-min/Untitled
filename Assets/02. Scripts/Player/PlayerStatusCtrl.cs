using UnityEngine;
using TMPro;

public class PlayerStatusCtrl : MonoBehaviour
{
    public DataManager Data { get; private set; }

    [SerializeField] private TMP_Text m_hp_text;
    [SerializeField] private TMP_Text m_mp_text; 
    
    private void Start()
    {
        Data = GetComponent<DataManager>();
        UpdateStatus();
        Debug.Log("업데이트함");
    }

    public void UpdateStatus()
    {
        m_hp_text.text = $"{Data.PlayerStat.HP}/{Data.MaxStat.HP} HP";
        m_mp_text.text = $"{Data.PlayerStat.MP}/{Data.MaxStat.MP} MP";
    }
}