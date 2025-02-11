using System.Collections;
using UnityEngine;

public class SwordCtrl : WeaponCtrl
{
    public PlayerCtrl Player { get; set; }
    public PlayerAttackState PlayerAttackInfo { get; private set; }


    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        PlayerAttackInfo = GameObject.Find("Player").GetComponent<PlayerAttackState>();

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );

        Trail.colorGradient = gradient;
        Trail.enabled = false;
    }

    public override void Use()
    {
        StopCoroutine(Attack());
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0f);
        m_area.enabled = true;
        Trail.enabled = true;

        yield return StartCoroutine(GetEnemies(0.2f));

        Damage();
        m_area.enabled = false;

        yield return new WaitForSeconds(0.4f);
        Trail.enabled = false;
    }

    private void Damage()
    {
        float damage = DataManager.Instance.Data.Stat.ATK;

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
