using System.Collections;
using Junyoung;
using UnityEngine;

public class PlayerSkill1 : MonoBehaviour
{
    private float m_damage;
    private float m_interval;

    private void Start()
    {
        m_damage = GameObject.Find("Player").GetComponent<PlayerCtrl>().Weapon.Info.Damage + GameObject.Find("DataManager").GetComponent<DataManager>().PlayerStat.ATK;
        m_interval = 1f;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            StartCoroutine(DamageOverTime(coll.gameObject));
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            StopCoroutine(DamageOverTime(coll.gameObject));
        }
    }

    private IEnumerator DamageOverTime(GameObject enemy)
    {
        while (enemy != null)
        {
            enemy.GetComponent<EnemyCtrl>().UpdateHP(-m_damage);

            yield return new WaitForSeconds(m_interval);
        }
    }
}
