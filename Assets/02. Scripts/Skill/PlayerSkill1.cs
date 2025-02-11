using System.Collections;
using Junyoung;
using UnityEngine;

public class PlayerSkill1 : MonoBehaviour
{
    private float m_interval = 1f;

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
            enemy.GetComponent<EnemyCtrl>().UpdateHP(-DataManager.Instance.Data.Stat.ATK);

            yield return new WaitForSeconds(m_interval);
        }
    }
}
