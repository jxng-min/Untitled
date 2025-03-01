using UnityEngine;
using UnityEngine.UIElements;

namespace Junyoung
{

    public class EnemyMeleeWeaponCtrl : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_box_size;
        [SerializeField]
        private Vector3 m_box_center;

        [SerializeField]
        protected EnemyCtrl m_enemy_ctrl;

        protected virtual void Awake()
        {
            m_enemy_ctrl = gameObject.transform.root.GetComponent<EnemyCtrl>();
        }

        // Update is called once per frame
        protected void Update()
        {
            Quaternion box_rotation = transform.rotation; // transform.rotation�� Quaternion ����
            Collider[] hit_colliders = Physics.OverlapBox(transform.TransformPoint(m_box_center), m_box_size, box_rotation);

            bool player_hit = false;

            foreach (Collider col in hit_colliders)
            {
                if (col.CompareTag("Player") && m_enemy_ctrl.StateContext.NowState is EnemyAttackState)
                {
                    player_hit= true;
                    break;
                }

            }
            m_enemy_ctrl.IsHit = player_hit;
            if(!m_enemy_ctrl.IsHit)
            {
                m_enemy_ctrl.IsHitting = false;
            }
        }

        protected void OnDrawGizmos()
        {
            if(m_enemy_ctrl&&m_enemy_ctrl.IsHit)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.matrix= Matrix4x4.TRS(transform.TransformPoint(m_box_center), transform.rotation,Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, m_box_size*2);
        }
    }
}