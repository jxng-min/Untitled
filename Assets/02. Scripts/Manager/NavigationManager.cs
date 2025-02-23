using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class NavigationManager : Singleton<NavigationManager>
{
    private LineRenderer m_line_renderer;
    private NavMeshAgent m_nav_agent;

    private Vector3 m_target_position;
    private Transform m_origin_transform;

    public string NavKeyName { get; private set; } = string.Empty;

    public void Init(Transform tr, Vector3 pos, float update_delay)
    {
        SetSource(tr);
        SetDestination(pos);

        m_line_renderer = GetComponent<LineRenderer>();
        m_line_renderer.enabled = true;
        m_line_renderer.startWidth = 0.5f;
        m_line_renderer.endWidth = 0.5f;
        m_line_renderer.positionCount = 0;

        m_nav_agent = GetComponent<NavMeshAgent>();
        m_nav_agent.isStopped = true;
        m_nav_agent.radius = 1f;
        m_nav_agent.height = 1f;

        StartCoroutine(UpdateNavi(update_delay));
    }

    private IEnumerator UpdateNavi(float update_delay)
    {
        WaitForSeconds delay = new WaitForSeconds(update_delay);

        while(true)
        {
            transform.position = m_origin_transform.position;
            m_nav_agent.SetDestination(m_target_position);

            DrawPath();

            yield return delay;
        }
    }

    public void SetDestination(Vector3 position)
    {
        m_target_position = position;
    }

    public void SetSource(Transform tr)
    {
        m_origin_transform = tr;
        transform.position = m_origin_transform.position;
    }

    private void DrawPath()
    {
        m_line_renderer.positionCount = m_nav_agent.path.corners.Length;
        m_line_renderer.SetPosition(0, transform.position);

        if(m_nav_agent.path.corners.Length < 2)
        {
            return;
        }
        
        for(int i = 1; i < m_nav_agent.path.corners.Length; i++)
        {
            m_line_renderer.SetPosition(i, m_nav_agent.path.corners[i]);
        }
    }

    public void StartNavigation(string quest_name, Transform player_transform, Vector3 destination)
    {
        NavKeyName = quest_name;

        Init(player_transform, destination, 0.01f);
    }

    public void TryStopNavigation(string quest_name)
    {
        if(NavKeyName == quest_name)
        {
            NavKeyName = string.Empty;

            m_nav_agent.isStopped = true;
            m_line_renderer.positionCount = 0;
            m_line_renderer.enabled = false;
        }
    }
}
