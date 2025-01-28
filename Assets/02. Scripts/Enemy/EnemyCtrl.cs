using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Junyoung
{
    public class EnemyCtrl : MonoBehaviour
    {
        //Enemt State
        IEnemyState<EnemyCtrl> m_enemy_idle_state;
        IEnemyState<EnemyCtrl> m_enemy_patrol_state;
        IEnemyState<EnemyCtrl> m_enemy_back_state;
        IEnemyState<EnemyCtrl> m_enemy_found_player_state;
        IEnemyState<EnemyCtrl> m_enemy_follow_state;
        IEnemyState<EnemyCtrl> m_enemy_get_damage_state;
        IEnemyState<EnemyCtrl> m_enemy_dead_state;
        IEnemyState<EnemyCtrl> m_enemy_ready_state;
        IEnemyState<EnemyCtrl> m_enemy_attack_state;

        public EnemyStateContext StateContext { get; private set; }

        //플레이어 오브젝트 접근용
        public GameObject Player;

        //Enemy 스탯
        [SerializeField]
        private EnemyStat m_origin_enemy_stat;
        public EnemyStat OriginEnemyStat { get {return m_origin_enemy_stat; } set { m_origin_enemy_stat=value; } }

        [SerializeField]
        private EnemyStat m_enemy_stat;
        public EnemyStat EnemyStat { get { return m_enemy_stat; } }

        //애니메이터
        public Animator Animator { get; private set; }
        
        //Nav
        public NavMeshAgent Agent { get; private set; }

        //순찰
        [SerializeField]
        private float m_patrol_range; // 인스펙터에서 접근하기 위해 변수 선언
        public float PatrolRange { get { return m_patrol_range; } set { m_patrol_range = value; } }

        [SerializeField]
        private Transform m_patrol_center;
        public Transform PatrolCenter { get { return m_patrol_center; } set { m_patrol_center = value; } }

        //플레이어 탐색
        public float DetectAngle { get; set; } = 45f; // 탐지 각도
        public float DetectDistance { get; set; } = 20f; // 탐지 거리
        public Vector3 DetectHeight { get; set; } = new Vector3(0, 2.0f, 0); //Ray 발사 위치 offset값
        public int RayCount { get; set; } = 20; // 발사되는 ray 수

        //추격
        public float FollowRadius { get; set; } = 25f; // 플레이어 발견 이후 추격하는 범위
        public Vector3 BackPosition { get; set; } //저장해두었다가 추격 종료시 복귀하는 위치


        //공격
        public bool CanAtk { get; set; } //공격 가능 여부
        public bool IsHit { get; set; }  // 공격이 적중 했는지 여부

        public float TotalAtkRate { get; set; } = 0; // 공격 애니메이션 재생 시간 + 공격간 대기 시간
        public float AttackDelay { get; set; } = 0f; // TotalAtkRate값에 도달하면 CanAtk을 활성화 시키는 값


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

            Player = GameObject.Find("Player");

            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();

            ChangeState(EnemyState.IDLE);
            InitStat();
        }

        public void InitStat() // 스탯,기본값 초기화
        {
            EnemyStat.HP = OriginEnemyStat.HP;
            EnemyStat.AtkDamege = OriginEnemyStat.AtkDamege;
            EnemyStat.AtkRate = OriginEnemyStat.AtkRate;
            EnemyStat.MoveSpeed = OriginEnemyStat.MoveSpeed;
            EnemyStat.AtkRange = OriginEnemyStat.AtkRange;

            CanAtk = true;
            IsHit = false;
            Agent.speed = EnemyStat.MoveSpeed;
        }

        void FixedUpdate()
        {
            //공격 쿨타임 계산
            if(TotalAtkRate >= AttackDelay)
            {
                AttackDelay += Time.deltaTime;
                if (CanAtk) CanAtk = false;
            }
            else
            {
                if (!CanAtk) CanAtk = true;
            }
            //FSM 패턴
            StateContext.OnStateUpdate();
        }

        public void ChangeState(EnemyState state) // State 전환
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

        public float GetAniLength(string clip_name) // 애니메이션 speed를 고려한 애니메이션 총 재생시간
        {
            RuntimeAnimatorController controller = Animator.runtimeAnimatorController;
            float length=1.0f;
            
            foreach (AnimationClip clip in controller.animationClips) // clip_name의 length 값을 반환
            {
                if (clip.name == clip_name)
                {
                    length = clip.length;
                    break;
                }
            }

            AnimatorStateInfo state_info = Animator.GetCurrentAnimatorStateInfo(0); // 현재 재생중인 상태의 정보 가져오기
            float speed=1.0f;

            if (state_info.IsName(clip_name))
            {
                speed = state_info.speed * state_info.speedMultiplier;
            }
            else
            {
                Debug.Log($"현재 상태가 '{clip_name}'과 일치하지 않습니다.");
            }

            //Debug.Log($"{clip_name}의 길이 : {length}");
            //Debug.Log($"{clip_name}의 속도 : {speed}");

            float real_length = length / speed;

            return real_length;
        }

        public void GetDamage(float damage)//피격시 호출
        {
            (m_enemy_get_damage_state as EnemyGetDamageState).Damage = damage;
            ChangeState(EnemyState.GETDAMAGE);
        }

        public void DetectPlayer() // RayCast를 사용한 플레이어 탐지 
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
