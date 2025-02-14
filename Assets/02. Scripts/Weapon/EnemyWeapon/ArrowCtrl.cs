using Junyoung;
using UnityEngine;
using UnityEngine.Pool;

namespace Junyoung
{
    public class ArrowCtrl : MonoBehaviour
    {

        public EnemyBowCtrl EnemyBowCtrl { get; set; }

        public IObjectPool<ArrowCtrl> ManagedArrowPool { get; set; }

        private void OnEnable()
        {
            Invoke("ReturnToPool", 3f);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player") && EnemyBowCtrl.StateContext.NowState is EnemyAttackState)
            {
                EnemyBowCtrl.IsHit = true;
                CancelInvoke("ReturnToPool");
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