using UnityEngine;
using System.Collections.Generic;
using Junyoung;

public abstract class WeaponCtrl : MonoBehaviour
{
    [SerializeField] protected Weapon m_info;
    public Weapon Info { get { return m_info; } set { m_info = value; } }

    [SerializeField] protected BoxCollider m_area;
    [SerializeField] protected TrailRenderer m_trail_effect;

    private HashSet<EnemyCtrl> m_enemies_set = new HashSet<EnemyCtrl>();
    private Queue<EnemyCtrl> m_enemies_queue = new Queue<EnemyCtrl>();

    public abstract void Use();

    protected void GetEnemy()
    {
        Collider[] colls = Physics.OverlapBox(transform.TransformPoint(m_area.center), m_area.size * 2f, transform.rotation);
        Debug.Log($"감지된 오브젝트의 수 : {colls.Length}");
        foreach (Collider coll in colls)
        {
            Debug.Log($"{coll.name}");            
            if(coll.CompareTag("Enemy"))
            {
                EnemyCtrl enemy = coll.GetComponent<EnemyCtrl>();
                //if(!m_enemies_set.Contains(enemy))
                {
                    m_enemies_set.Add(enemy);
                    m_enemies_queue.Enqueue(enemy);
                }
            }
        }

        Debug.Log($"감지된 적의 수 : {m_enemies_queue.Count}");
    }

    protected void DropEnemy(float damage)
    {
        while(m_enemies_queue.Count > 0)
        {
            Debug.Log($"큐에 남은 적의 수 : {m_enemies_queue.Count}");
            var enemy = m_enemies_queue.Dequeue();
            enemy.GetDamage(damage);
            Debug.Log($"데미지를 {damage}만큼 {enemy}에게 입힘");
            m_enemies_set.Remove(enemy);
        }
    }
}
