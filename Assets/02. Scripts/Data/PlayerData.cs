using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private int m_player_level = 1;
    public int Level
    {
        get { return m_player_level; }
        set { m_player_level = value; }
    }

    [SerializeField] private float m_player_exp = 0f;
    public float EXP
    {
        get { return m_player_exp; }
        set { m_player_exp = value; }
    }


    [SerializeField] private Vector3 m_player_current_position;
    public Vector3 Position
    {
        get { return m_player_current_position; }
        set { m_player_current_position = value; }
    }

    [SerializeField] private Stat m_player_stat;
    public Stat Stat
    {
        get { return m_player_stat; }
        set { m_player_stat = value; }
    }

    public PlayerData()
    {
        Level = 1;
        EXP = 0f;

        Position = new Vector3(101f, 0f, 15f);

        Stat = new Stat(50f, 30f, 20f, 0.9f, 1f);
    }
}