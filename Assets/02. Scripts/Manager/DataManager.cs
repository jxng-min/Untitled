using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private Stat m_growth_player_stat;
    [SerializeField] private Stat m_default_player_stat;

    private StatData m_player_stat;
    public StatData PlayerStat
    {
        get { return m_player_stat; }
        set { m_player_stat = value; }
    }
    
    private StatData m_max_stat;
    public StatData MaxStat
    {
        get { return m_max_stat; }
        set { m_max_stat = value; }
    }

    public PlayerStatusCtrl StatusUI { get; private set; }

    private void Awake()
    {
        PlayerStat = new StatData(m_default_player_stat);
        MaxStat = new StatData(m_default_player_stat);

        StatusUI = GetComponent<PlayerStatusCtrl>();
    }
}
