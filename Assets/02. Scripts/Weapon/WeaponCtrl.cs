using UnityEngine;
using System.Collections.Generic;
using Junyoung;
using System.Collections;

public abstract class WeaponCtrl : MonoBehaviour
{
    [SerializeField] protected Weapon m_info;
    public Weapon Info { get { return m_info; } set { m_info = value; } }

    [SerializeField] protected BoxCollider m_area;
    [SerializeField] protected TrailRenderer m_trail_effect;

    protected Queue<EnemyCtrl> m_enemies_queue = new Queue<EnemyCtrl>();
    protected HashSet<EnemyCtrl> m_enemies_set = new HashSet<EnemyCtrl>();

    public abstract void Use();

    protected IEnumerator GetEnemies(float target_time)
    {
        float elapsed_time = 0f;

        while(elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            Collider[] colliders = Physics.OverlapBox(transform.TransformPoint(m_area.center), m_area.size * 2f, transform.rotation);
            foreach(var collider in colliders)
            {
                if(collider.CompareTag("Enemy"))
                {
                    var enemy = collider.GetComponent<EnemyCtrl>();
                    if(!m_enemies_set.Contains(enemy))
                    {
                        m_enemies_queue.Enqueue(enemy);
                        m_enemies_set.Add(enemy);
                    }
                }
            }

            yield return null;
        }

        Debug.Log($"몬스터 큐에 들어가 있는 요소의 개수: {m_enemies_queue.Count}");
    }

    protected void DestroyEnemies(float damage)
    {
        while(m_enemies_queue.Count > 0)
        {
            var enemy = m_enemies_queue.Dequeue();
            enemy.GetDamage(damage);
            m_enemies_set.Remove(enemy);

            Debug.Log($"적에게 {damage}의 피해를 주었다.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix= Matrix4x4.TRS(transform.TransformPoint(m_area.center), transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, m_area.size * 2f);
    }
}
