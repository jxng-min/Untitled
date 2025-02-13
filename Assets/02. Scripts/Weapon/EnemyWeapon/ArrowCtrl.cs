using Junyoung;
using UnityEngine;
using UnityEngine.Pool;

namespace Junyoung
{
    public class ArrowCtrl : MonoBehaviour
    {

        EnemyBowCtrl m_enemy_bow_ctrl;

        public IObjectPool<ArrowCtrl> ManagedArrowPool { get; set; }

        private void OnEnable()
        {
            //Invoke("ReturnToPool", 3f);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!m_enemy_bow_ctrl)
            {
                m_enemy_bow_ctrl = gameObject.transform.root.GetComponent<EnemyBowCtrl>();
            }

            if (col.CompareTag("Player") && m_enemy_bow_ctrl.StateContext.NowState is EnemyAttackState)
            {
                m_enemy_bow_ctrl.IsHit = true;
                //CancelInvoke("ReturnToPool");
                ReturnToPool();
            }
        }


        public void SetArrowPool(IObjectPool<ArrowCtrl> pool)
        {
            ManagedArrowPool = pool;
        }

        public void ReturnToPool()
        {
            ManagedArrowPool.Release(this);
        }

    }
}