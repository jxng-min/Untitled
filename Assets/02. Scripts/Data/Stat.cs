using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float m_health_point;
    public float HP
    {
        get { return m_health_point; }
        set { m_health_point = value; }
    }
    
    [SerializeField] private float m_mana_point;
    public float MP
    {
        get { return m_mana_point; }
        set { m_mana_point = value; }
    }

    [SerializeField] private float m_attack_point;
    public float ATK
    {
        get { return m_attack_point; }
        set { m_attack_point = value; }
    }

    [SerializeField] private float m_attack_rate;
    public float Rate
    {
        get { return m_attack_rate; }
        set { m_attack_rate = value; }
    }

    [SerializeField] private float m_defense_point;
    public float DEF
    {
        get { return m_defense_point; }
        set { m_defense_point = value; }
    }

    public Stat()
    {
        HP = 0f;
        MP = 0f;
        ATK = 0f;
        Rate = 0f;
        DEF = 0f;
    }

    public Stat(float hp, float mp, float atk, float rate, float def)
    {
        HP = hp;
        MP = mp;
        ATK = atk;
        Rate = rate;
        DEF = def;
    }

    public static Stat operator+(Stat stat1, Stat stat2)
    {
        Stat temp_stat = new Stat();

        temp_stat.HP = stat1.HP + stat2.HP;
        temp_stat.MP = stat1.MP + stat2.MP;
        temp_stat.ATK = stat1.ATK + stat2.ATK;
        temp_stat.Rate = stat1.Rate + stat2.Rate;
        temp_stat.DEF = stat1.DEF + stat2.DEF;

        return temp_stat;
    }

    public static Stat operator+(Stat stat1, EquipmentEffect effect)
    {
        Stat temp_stat = new Stat();

        temp_stat.HP = stat1.HP;
        temp_stat.MP = stat1.MP;
        temp_stat.ATK = stat1.ATK + effect.ATK;
        temp_stat.Rate = stat1.Rate + effect.Rate;
        temp_stat.DEF = stat1.DEF + effect.DEF;

        return temp_stat;
    }

    public static Stat operator*(Stat stat, int level)
    {
        Stat temp_stat = stat;

        temp_stat.HP *= level;
        temp_stat.MP *= level;
        temp_stat.ATK *= level;
        temp_stat.DEF *= level;

        return temp_stat;
    }
}