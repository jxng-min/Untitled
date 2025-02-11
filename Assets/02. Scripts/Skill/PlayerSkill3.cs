using Junyoung;
using UnityEngine;

public class PlayerSkill3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            coll.GetComponent<EnemyCtrl>().UpdateHP(-DataManager.Instance.Data.Stat.ATK);
        }
    }
}
