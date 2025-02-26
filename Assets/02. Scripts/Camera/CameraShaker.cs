using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private bool m_is_enable = false;
    public bool IsEnable
    {
        get { return m_is_enable; }
        set { m_is_enable = value; }
    }

    private Vector3 m_origin_local_position;

    private void Awake()
    {
        m_origin_local_position = transform.localPosition;
    } 

    public void Shaking(float range, float time)
    {
        if(IsEnable)
        {
            StartCoroutine(ShakingCoroutine(range, time));
        }
    }

    private IEnumerator ShakingCoroutine(float range, float time)
    {
        float elapsed_time = 0f;

        while(elapsed_time <= time)
        {
            transform.localPosition = Random.insideUnitSphere * range + m_origin_local_position;

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = m_origin_local_position;
    }
}
