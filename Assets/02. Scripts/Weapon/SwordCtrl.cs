using System.Collections;
using Junyoung;
using UnityEngine;

public class SwordCtrl : WeaponCtrl
{
    private DataManager m_data_manager; 
    public PlayerAttackState Player { get; private set; }

    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerAttackState>();
        m_data_manager = GameObject.Find("DataManager").GetComponent<DataManager>();
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
        //m_trail_effect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        m_area.enabled = false;

        yield return new WaitForSeconds(0.3f);
        //m_trail_effect.enabled = false;
    }

    private void OnTriggerEnter(Collider obj)
    {
        if(obj.CompareTag("Enemy"))
        {
            float damage = m_data_manager.PlayerStat.ATK + Info.Damage;
            switch(Player.ComboIndex)
            {
                case 1:
                damage += damage * 0.2f;
                    break;
                
                case 2:
                damage += damage * 0.3f;
                    break;

                default:
                    break;
            }

            obj.GetComponent<EnemyCtrl>().GetDamage(damage);
        }
    }
}
