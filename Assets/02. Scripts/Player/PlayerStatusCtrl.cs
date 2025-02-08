using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusCtrl : MonoBehaviour
{
    public DataManager Data { get; private set; }

    [SerializeField] private Slider m_hp_slider;
    [SerializeField] private Slider m_mp_slider;
    [SerializeField] private Slider m_exp_slider;

    
    private void Start()
    {
        Data = GetComponent<DataManager>();
    }

    public void Update()
    {
        m_hp_slider.value = Mathf.Lerp(m_hp_slider.value, Data.PlayerStat.HP / Data.MaxStat.HP, Time.deltaTime * 20f);
        m_mp_slider.value = Mathf.Lerp(m_mp_slider.value, Data.PlayerStat.MP / Data.MaxStat.MP, Time.deltaTime * 20f);
    }
}