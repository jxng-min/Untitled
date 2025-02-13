using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamageIndicator : MonoBehaviour
{
    [Header("Prefab's Rigidbody")]
    [SerializeField] private Rigidbody m_rigidbody;

    [Header("Prefab's Damage Label")]
    [SerializeField] private TMP_Text m_damage_label;

    [Header("Prefab's Child Object")]
    [SerializeField] private Transform m_child_transform;

    private Coroutine m_coroutine_fade_label;

    private void Update()
    {
        m_child_transform.LookAt(Camera.main.transform);
    }

    public void Init(Vector3 pos, float amount, Color color, float size = 1f, bool miss_flag = false)
    {
        m_rigidbody.angularVelocity = Vector3.zero;
        m_rigidbody.linearVelocity = Vector3.zero;
        transform.localScale = Vector3.one * size;

        transform.position = pos;

        if(miss_flag)
        {
            m_damage_label.text = "DODGE";
        }
        else
        {
            m_damage_label.text = Mathf.Abs(Mathf.RoundToInt(amount)).ToString();
        }

        m_damage_label.color = color;

        m_rigidbody.AddForce(new Vector3(Random.Range(-2f, 2f), 5f, Random.Range(-2f, 2f)), ForceMode.Impulse);

        if(m_coroutine_fade_label is not null)
        {
            StopCoroutine(m_coroutine_fade_label);
        }
        m_coroutine_fade_label = StartCoroutine(CoroutineFadeLabel());
    }

    private IEnumerator CoroutineFadeLabel()
    {
        float elapsed_time = 0f;
        Vector3 init_size = transform.localScale;

        while(elapsed_time < 1f)
        {
            elapsed_time += Time.deltaTime;

            transform.localScale = Vector3.Lerp(init_size, Vector3.zero, elapsed_time);
            m_damage_label.alpha = Mathf.Lerp(1f, 0f, elapsed_time);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}