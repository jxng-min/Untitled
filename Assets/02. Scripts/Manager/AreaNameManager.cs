using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class AreaNameManager : MonoBehaviour
{
    public TextMeshProUGUI m_area_name;
    private float m_fade_time = 5f;

    private Dictionary<Collider,string> m_areas= new Dictionary<Collider,string>();

    private Coroutine m_fade_coroutine;

    private Collider m_default_area;
    void Start()
    {
        GameObject canvas = GameObject.Find("Overlay Canvas");

        TextMeshProUGUI[] texts = canvas.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name == "Area Name")
            {
                m_area_name = text;
            }
        }

        m_area_name.gameObject.SetActive(false);

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            if (col.gameObject.name == "Area1")
            {
                m_areas[col] = "[ 잊혀진 숲길 ]";
                m_default_area = col;
            }
            if (col.gameObject.name == "Area2") m_areas[col] = "[ 고요한 정착지 ]";
            if (col.gameObject.name == "Area3") m_areas[col] = "[ 멸망한 왕국의 폐허 ]";
            Debug.Log(col.gameObject.name);
        }
    }

    public void TriggerEnter(Collider object_col, Collider area)
    {
        if (object_col.CompareTag("Player") && m_areas.ContainsKey(area))
        {
            ShowAreaName(area);
        }
    }

    public void TriggerExit(Collider object_col, Collider area)
    {
        if (object_col.CompareTag("Player") && m_areas.ContainsKey(area))
        {
           if(area != m_default_area)
            {
                ShowAreaName(m_default_area);
            }
        }
    }

    private void ShowAreaName(Collider area)
    {
        m_area_name.text = m_areas[area];
        m_area_name.gameObject.SetActive(true);
        m_area_name.color = new Color(m_area_name.color.r, m_area_name.color.g, m_area_name.color.b, 1f);
        if (m_fade_coroutine != null)
        {
            StopCoroutine(m_fade_coroutine);
        }

        m_fade_coroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);

        float now_time = 0f;

        while (now_time < m_fade_time)
        {
            now_time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, now_time / m_fade_time);
            m_area_name.color = new Color(m_area_name.color.r, m_area_name.color.g, m_area_name.color.b, alpha);
            yield return null;
        }

        m_area_name.gameObject.SetActive(false);
    }

}
