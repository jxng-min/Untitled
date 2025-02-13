using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Junyoung
{
    public class ArrowFactory : MonoBehaviour
    {
        private ObjectPool<ArrowCtrl> m_arrow_pools;
        [SerializeField]
        private GameObject m_arrow_prefab;

        private void Awake()
        {
            m_arrow_pools = new ObjectPool<ArrowCtrl> (CreateArrow, OnGetArrow, OnReturnArrow, OnDestoryArrow, maxSize : 10);

        }

        public ArrowCtrl SpawnArrow(Transform spawn_pos, EnemyCtrl enemy)
        {
            ArrowCtrl new_arrow = m_arrow_pools.Get();
            new_arrow.transform.position = spawn_pos.position;
            new_arrow.transform.rotation = spawn_pos.rotation;
            new_arrow.transform.SetParent(enemy.transform);
            return new_arrow;
        }
        private ArrowCtrl CreateArrow()
        {
            var new_arrow = Instantiate(m_arrow_prefab).GetComponent<ArrowCtrl>();
            new_arrow.SetArrowPool(m_arrow_pools);
            return new_arrow;
        }

        private void OnGetArrow(ArrowCtrl arrow)
        {
            arrow.gameObject.SetActive(true);
        }

        public void OnReturnArrow(ArrowCtrl arrow)
        {
            arrow.transform.SetParent(null);
            arrow.gameObject.SetActive(false);
        }

        private void OnDestoryArrow(ArrowCtrl arrow)
        {
            Destroy(arrow.gameObject);
        }
    }
}