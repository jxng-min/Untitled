using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Vector3 m_origin_local_position;

    private void Awake()
    {
        m_origin_local_position = transform.localPosition;
    } 

    public void Shaking(float range, float time)
    {
        StartCoroutine(ShakingCoroutine(range, time));
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
