using Junyoung;
using UnityEngine;
using UnityEngine.Pool;

namespace Junyoung
{
    public class ArrowCtrl : EnemyWeaponCtrl
    {
        [SerializeField]
        private Transform m_arrow_transfrom;

        public IObjectPool<ArrowCtrl> ManagedArrowPool { get; set; }

        public void SetArrowPool(IObjectPool<ArrowCtrl> pool)
        {
            ManagedArrowPool = pool;
        }

        public void ReturnToPool()
        {
            ManagedArrowPool.Release(this);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}