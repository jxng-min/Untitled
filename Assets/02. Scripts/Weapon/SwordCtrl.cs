using System.Collections;
using UnityEngine;

public class SwordCtrl : WeaponCtrl
{
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
}
