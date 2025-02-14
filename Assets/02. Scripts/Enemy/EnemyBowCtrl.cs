using UnityEngine;
using UnityEngine.Pool;

namespace Junyoung
{

    public class EnemyBowCtrl : EnemyCtrl
    {

        public Transform m_arrow_spawn_pos;

        public new IObjectPool<EnemyBowCtrl> ManagedPool { get; set; }
            

        public override void InitComponent()
        {
            base.InitComponent();
            base.m_enemy_attack_state = gameObject.AddComponent<EnemyBowAttackState>();
        }


        public void SetEnemyPool(IObjectPool<EnemyBowCtrl> pool)
        {
            Debug.Log("BowǮ �ʱ�ȭ");
            this.ManagedPool = pool;
        }

        public override void ReturnToPool()
        {
            Debug.Log($"{this.name} ��ȯ (Bow)");
            ManagedPool.Release(this as EnemyBowCtrl);  // ��Ȯ�� Ÿ���� ����Ͽ� Ǯ�� ��ȯ
        }
    }
}