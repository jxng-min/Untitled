using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "Scriptable Object/EnemyStat")]
public class EnemyStat : ScriptableObject
{
    [SerializeField]
    private float m_hp;
    public float HP { get { return m_hp; } set { m_hp = value; } }
    
    [SerializeField]
    private float m_move_speed;
    public float MoveSpeed { get { return m_move_speed; } set { m_move_speed = value;} }

    [SerializeField]
    private float m_atk_damage;
    public float AtkDamege { get { return m_atk_damage; } set { m_atk_damage = value; } }

    [SerializeField]
    private float m_atk_rate;
    public float AtkRate { get { return m_atk_rate; } set { m_atk_rate = value;} }

    [SerializeField]
    private float m_atk_range;
    public float AtkRange { get { return m_atk_range; } set { m_atk_range = value;} }

    [SerializeField]
    private float m_detect_range;
    public float DetectRange { get { return m_detect_range;} set { m_detect_range = value;} }

    [SerializeField]
    private float m_follow_range;
    public float FollowRange { get { return m_follow_range;} set { m_follow_range = value;} }
}
