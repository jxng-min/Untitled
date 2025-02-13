using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusCtrl : MonoBehaviour
{
    [SerializeField] private Slider m_hp_slider;
    [SerializeField] private Slider m_mp_slider;
    [SerializeField] private Slider m_exp_slider;
    [SerializeField] private TMP_Text m_level_label;

    public void Update()
    {
        m_hp_slider.value = Mathf.Lerp(m_hp_slider.value, DataManager.Instance.Data.Stat.HP / DataManager.Instance.GetMaxStat().HP, Time.deltaTime * 20f);
        m_mp_slider.value = Mathf.Lerp(m_mp_slider.value, DataManager.Instance.Data.Stat.MP / DataManager.Instance.GetMaxStat().MP, Time.deltaTime * 20f);

        if(DataManager.Instance.Data.EXP < DataManager.Instance.GetMaxExp(DataManager.Instance.Data.Level))
        {
            m_exp_slider.value = Mathf.Lerp(m_exp_slider.value, DataManager.Instance.Data.EXP / DataManager.Instance.GetMaxExp(DataManager.Instance.Data.Level), Time.deltaTime * 20f);
        }
        else
        {
            m_exp_slider.value = 0f;
            
            DataManager.Instance.Data.Level++;
            DataManager.Instance.Data.EXP = 0f;

            DataManager.Instance.UpdateStat();

            DataManager.Instance.Data.Stat.HP = DataManager.Instance.GetMaxStat().HP;
            DataManager.Instance.Data.Stat.MP = DataManager.Instance.GetMaxStat().MP;
        }

        m_level_label.text = DataManager.Instance.Data.Level.ToString();
    }
}