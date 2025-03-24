using UnityEngine;

[System.Serializable]
public class SEnemyStat 
{
    public float m_hp;
    public float m_move_speed;
    public float m_atk_damage;
    public float m_atk_rate;
    public float m_atk_range;
    public float m_detect_range;
    public float m_follow_range;

    public SEnemyStat(EnemyStat stat)
    {
        m_hp = stat.HP;
        m_move_speed= stat.MoveSpeed;
        m_atk_damage = stat.AtkDamege;
        m_atk_rate= stat.AtkRate;
        m_atk_range= stat.AtkRange;
        m_detect_range= stat.DetectRange;
        m_follow_range= stat.FollowRange;
    }

    public EnemyStat ToEnemyStat()
    {
        EnemyStat stat = new EnemyStat();
        stat.HP = m_hp;
        stat.MoveSpeed= m_move_speed;
        stat.AtkDamege= m_atk_damage;
        stat.AtkRate= m_atk_rate;
        stat.AtkRange= m_atk_range;
        stat.DetectRange= m_detect_range;
        stat.FollowRange= m_follow_range;
        return stat;
    }
}
