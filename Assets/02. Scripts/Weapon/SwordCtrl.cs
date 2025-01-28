using System.Collections;
using UnityEngine;

public class SwordCtrl : WeaponCtrl
{
    private DataManager m_data_manager; 
    public PlayerCtrl Player { get; private set; }
    public PlayerAttackState PlayerAttackInfo { get; private set; }
    public float FinalDamage { get; set; }

    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        PlayerAttackInfo = GameObject.Find("Player").GetComponent<PlayerAttackState>();
        m_data_manager = GameObject.Find("DataManager").GetComponent<DataManager>();

        FinalDamage = m_data_manager.PlayerStat.ATK + Info.Damage;
    }

    public override void Use()
    {
        StopCoroutine(Attack1());
        StartCoroutine(Attack1());
    }

    private IEnumerator Attack1()
    {
        yield return new WaitForSeconds(0.1f);
        m_area.enabled = true;

        yield return new WaitForSeconds(0.05f);
        GetEnemy();
        yield return new WaitForSeconds(Player.AttackSpeed);
        yield return StartCoroutine(DamageToEnemy(1));

        m_area.enabled = false;
    }

    private IEnumerator DamageToEnemy(int index)
    {
        yield return null;

        float damage = FinalDamage;
        switch(index)
        {
            case 1:
                damage += FinalDamage * 0.1f;
                break;
            
            case 2:
                damage += FinalDamage * 0.3f;
                break;

            default:
                break;
        }

        DropEnemy(damage);
    }
}
