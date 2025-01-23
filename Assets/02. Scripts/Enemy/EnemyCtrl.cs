using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Junyoung
{
    public class EnemyCtrl : MonoBehaviour
    {
        IEnemyState<EnemyCtrl> m_enemy_idle_state;
        IEnemyState<EnemyCtrl> m_enemy_patrol_state;
        IEnemyState<EnemyCtrl> m_enemy_back_state;
        IEnemyState<EnemyCtrl> m_enemy_follow_state;
        IEnemyState<EnemyCtrl> m_enemy_get_damage_state;
        IEnemyState<EnemyCtrl> m_enemy_dead_state;
        IEnemyState<EnemyCtrl> m_enemy_ready_state;
        IEnemyState<EnemyCtrl> m_enemy_attack_state;

        public EnemyStateContext StateContext { get; private set; }

        public Animator Animator { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        [SerializeField]
        private float m_patrol_range; // 인스펙터에서 접근하기 위해 변수 선언
        public float PatrolRange { get { return m_patrol_range; } set { m_patrol_range = value; } }

        [SerializeField]
        private Transform m_patrol_center;
        public Transform PatrolCenter { get { return m_patrol_center; } set { m_patrol_center = value; } }


        void Start()
        {
            m_enemy_idle_state = gameObject.AddComponent<EnemyIdleState>();
            m_enemy_attack_state= gameObject.AddComponent<EnemyAttackState>();
            m_enemy_back_state= gameObject.AddComponent<EnemyBackState>();
            m_enemy_dead_state= gameObject.AddComponent<EnemyDeadState>();
            m_enemy_follow_state= gameObject.AddComponent<EnemyFollowState>();
            m_enemy_get_damage_state= gameObject.AddComponent<EnemyGetDamageState>();
            m_enemy_patrol_state= gameObject.AddComponent<EnemyPatrolState>();
            m_enemy_ready_state= gameObject.AddComponent<EnemyReadyState>();

            StateContext= new EnemyStateContext(this);

            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();

            ChangeState(EnemyState.PATROL);
        }

        // Update is called once per frame
        void Update()
        {
            StateContext.OnStateUpdate();
        }

        public void ChangeState(EnemyState state)
        {
            switch (state)
            {
                case EnemyState.IDLE:
                    StateContext.Transition(m_enemy_idle_state); break;
                case EnemyState.ATTACK:
                    StateContext.Transition(m_enemy_attack_state); break;
                case EnemyState.BACK:
                    StateContext.Transition(m_enemy_back_state); break;
                case EnemyState.PATROL:
                    StateContext.Transition(m_enemy_patrol_state); break;
                case EnemyState.READY:
                    StateContext.Transition(m_enemy_ready_state); break;
                case EnemyState.FOLLOW:
                    StateContext.Transition(m_enemy_follow_state); break;
                case EnemyState.GETDAMAGE:
                    StateContext.Transition(m_enemy_get_damage_state); break;
                case EnemyState.DEAD:
                    StateContext.Transition(m_enemy_dead_state); break;
            }
        }
    }
}
