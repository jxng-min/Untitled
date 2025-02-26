using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private AreaNameManager m_area_manager;

    private void Start()
    {
        m_area_manager = GetComponentInParent<AreaNameManager>(); // �θ𿡼� AreaNameManager ã��
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_area_manager != null)
        {
            m_area_manager.TriggerEnter(other, this.GetComponent<Collider>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_area_manager != null)
        {
            m_area_manager.TriggerExit(other, this.GetComponent<Collider>());
        }
    }
}