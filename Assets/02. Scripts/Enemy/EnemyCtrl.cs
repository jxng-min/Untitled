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
        IEnemyState<EnemyCtrl> m_enemy_found_player_state;
        IEnemyState<EnemyCtrl> m_enemy_follow_state;
        IEnemyState<EnemyCtrl> m_enemy_get_damage_state;
        IEnemyState<EnemyCtrl> m_enemy_dead_state;
        IEnemyState<EnemyCtrl> m_enemy_ready_state;
        IEnemyState<EnemyCtrl> m_enemy_attack_state;

        public GameObject Player;

        [SerializeField]
        private EnemyStat m_origin_enemy_stat;
        public EnemyStat OriginEnemyStat { get {return m_origin_enemy_stat; } set { m_origin_enemy_stat=value; } }

        [SerializeField]
        private EnemyStat m_enemy_stat;
        public EnemyStat EnemyStat { get { return m_enemy_stat; } }

        public EnemyStateContext StateContext { get; private set; }
        public Animator Animator { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        [SerializeField]
        private float m_patrol_range; // 인스펙터에서 접근하기 위해 변수 선언
        public float PatrolRange { get { return m_patrol_range; } set { m_patrol_range = value; } }

        [SerializeField]
        private Transform m_patrol_center;
        public Transform PatrolCenter { get { return m_patrol_center; } set { m_patrol_center = value; } }

        public float DetectAngle { get; set; } = 45f;
        public float DetectDistance { get; set; } = 20f;
        public Vector3 DetectHeight { get; set; } = new Vector3(0, 2.0f, 0);
        public int RayCount { get; set; } = 20;

        public float FollowRadius { get; set; } = 25f;
        public float CombatRadius { get; set; } = 5f;

        public Vector3 BackPosition { get; set; }

        public bool CanAtk { get; set; }

        public float TotalAtkRate { get; set; }


        void Start()
        {
            m_enemy_idle_state = gameObject.AddComponent<EnemyIdleState>();
            m_enemy_attack_state= gameObject.AddComponent<EnemyAttackState>();
            m_enemy_back_state= gameObject.AddComponent<EnemyBackState>();
            m_enemy_dead_state= gameObject.AddComponent<EnemyDeadState>();
            m_enemy_found_player_state = gameObject.AddComponent<EnemyFoundPlayerState>();
            m_enemy_follow_state = gameObject.AddComponent<EnemyFollowState>();
            m_enemy_get_damage_state= gameObject.AddComponent<EnemyGetDamageState>();
            m_enemy_patrol_state= gameObject.AddComponent<EnemyPatrolState>();
            m_enemy_ready_state= gameObject.AddComponent<EnemyReadyState>();

            StateContext= new EnemyStateContext(this);

            Player =GameObject.FindWithTag("Player");

            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();

            ChangeState(EnemyState.IDLE);
            InitStat();
        }

        public void InitStat()
        {
            EnemyStat.HP = OriginEnemyStat.HP;
            EnemyStat.AtkDamege = OriginEnemyStat.AtkDamege;
            EnemyStat.AtkRate = OriginEnemyStat.AtkRate;
            EnemyStat.MoveSpeed = OriginEnemyStat.MoveSpeed;
            EnemyStat.AtkRange = OriginEnemyStat.AtkRange;
            EnemyStat.AtkAniLength= OriginEnemyStat.AtkAniLength;
            EnemyStat.AtkAniSpeed = OriginEnemyStat.AtkAniSpeed;

            CanAtk = true;
            TotalAtkRate = EnemyStat.AtkAniLength / EnemyStat.AtkAniSpeed + EnemyStat.AtkRate; // 애니메이션이 재생 시간 + 개체의 공격 쿨타임
            Agent.speed = EnemyStat.MoveSpeed;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(TotalAtkRate >= 0)
            {
                TotalAtkRate -= Time.deltaTime;
            }
            else
            {
                if(!CanAtk) CanAtk = true;
            }
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
                case EnemyState.FOUNDPLAYER:
                    StateContext.Transition(m_enemy_found_player_state); break;
                case EnemyState.FOLLOW:
                    StateContext.Transition(m_enemy_follow_state); break;
                case EnemyState.GETDAMAGE:
                    StateContext.Transition(m_enemy_get_damage_state); break;
                case EnemyState.DEAD:
                    StateContext.Transition(m_enemy_dead_state); break;
            }
        }

        public void DetectPlayer()
        {
            float start_angle = -DetectAngle;
            float offset_angle = DetectAngle / RayCount;

            for(int i =0; i< RayCount; i++)
            {
                float angle = start_angle + offset_angle * i;
                Vector3 dir = Quaternion.Euler(0,angle, 0) * transform.forward;

                Ray ray = new Ray(transform.position + DetectHeight, dir);
                if (Physics.Raycast(ray, out RaycastHit hit, DetectDistance))
                {
                    if(hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("플레이어 감지");
                        ChangeState(EnemyState.FOUNDPLAYER);
                        Debug.DrawRay(transform.position + DetectHeight, dir * DetectDistance, Color.red);
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position + DetectHeight, dir * DetectDistance, Color.green);
                }
            }
        }
    }
}
