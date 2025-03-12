using UnityEngine;
using Junyoung;

public class PlayerSkill5 : MonoBehaviour
{
    private float m_stun_time = 10f;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            //coll.GetComponent<EnemyCtrl>().GetStun(m_stun_time);
        }
    }
}
