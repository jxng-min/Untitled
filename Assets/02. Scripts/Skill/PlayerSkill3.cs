using Junyoung;
using UnityEngine;

public class PlayerSkill3 : MonoBehaviour
{
    private float m_damage;

    private void Start()
    {
        m_damage = GameObject.Find("Player").GetComponent<PlayerCtrl>().AttackPower;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            coll.GetComponent<EnemyCtrl>().UpdateHP(-m_damage);
        }
    }
}
