using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private Stat m_player_stat;
    public Stat PlayerStat
    {
        get { return m_player_stat; }
        set { m_player_stat = value; }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
