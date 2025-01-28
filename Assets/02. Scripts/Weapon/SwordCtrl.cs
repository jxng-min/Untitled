using System.Collections;
using UnityEngine;

public class SwordCtrl : WeaponCtrl
{
    private DataManager m_data_manager; 
    public PlayerCtrl Player { get; set; }
    public PlayerAttackState PlayerAttackInfo { get; private set; }


    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        PlayerAttackInfo = GameObject.Find("Player").GetComponent<PlayerAttackState>();
        m_data_manager = GameObject.Find("DataManager").GetComponent<DataManager>();
    }

    public override void Use()
    {
        StopCoroutine(Attack());
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.1f);
        m_area.enabled = true;

        yield return StartCoroutine(GetEnemies(0.5f));

        Damage();
        m_area.enabled = false;
    }

    private void Damage()
    {
        float damage = m_data_manager.PlayerStat.ATK + Info.Damage;

        switch(PlayerAttackInfo.ComboIndex)
        {
            case 1:
                damage += damage * 0.1f;
                break;

            case 2:
                damage += damage * 0.2f;
                break;

            default:
                break;
        }

        DestroyEnemies(damage);
    }
}
