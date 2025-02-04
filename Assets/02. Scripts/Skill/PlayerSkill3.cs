using System.Collections;
using Junyoung;
using UnityEngine;

public class PlayerSkill3 : MonoBehaviour
{
    private float m_damage;

    private void Start()
    {
        m_damage = GameObject.Find("Player").GetComponent<PlayerCtrl>().Weapon.Info.Damage + GameObject.Find("DataManager").GetComponent<DataManager>().PlayerStat.ATK;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            coll.GetComponent<EnemyCtrl>().GetDamage(m_damage);
        }
    }
}
