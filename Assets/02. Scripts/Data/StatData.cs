public class StatData
{
    private float m_health_point;
    public float HP
    {
        get { return m_health_point; }
        set { m_health_point = value; }
    }
    
    private float m_mana_point;
    public float MP
    {
        get { return m_mana_point; }
        set { m_mana_point = value;}
    }

    private float m_attack_point;
    public float ATK
    {
        get { return m_attack_point; }
        set { m_attack_point = value; }
    }

    private float m_attack_rate;
    public float Rate
    {
        get { return m_attack_rate; }
        set { m_attack_rate = value; }
    }

    private float m_defense_point;
    public float DEF
    {
        get { return m_defense_point; }
        set { m_defense_point = value; }
    }

    public StatData(Stat player_stat)
    {
        HP = player_stat.HP;
        MP = player_stat.MP;
        ATK = player_stat.ATK;
        Rate = player_stat.Rate;
        DEF = player_stat.DEF;
    }
}
