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

        //�÷��̾� ������Ʈ ���ٿ�
        public GameObject Player;

        //Enemy ����
        [SerializeField]
        private EnemyStat m_origin_enemy_stat;
        public EnemyStat OriginEnemyStat { get {return m_origin_enemy_stat; } set { m_origin_enemy_stat=value; } }

        [SerializeField]
        private EnemyStat m_enemy_stat;
        public EnemyStat EnemyStat { get { return m_enemy_stat; } }

        //�ִϸ�����
        public Animator Animator { get; private set; }
        
        //Nav
        public NavMeshAgent Agent { get; private set; }

        //����
        [SerializeField]
        private float m_patrol_range; // �ν����Ϳ��� �����ϱ� ���� ���� ����
        public float PatrolRange { get { return m_patrol_range; } set { m_patrol_range = value; } }

        [SerializeField]
        private Transform m_patrol_center;
        public Transform PatrolCenter { get { return m_patrol_center; } set { m_patrol_center = value; } }

        //�÷��̾� Ž��
        public float DetectAngle { get; set; } = 45f; // Ž�� ����
        public float DetectDistance { get; set; } = 20f; // Ž�� �Ÿ�
        public Vector3 DetectHeight { get; set; } = new Vector3(0, 2.0f, 0); //Ray �߻� ��ġ offset��
        public int RayCount { get; set; } = 20; // �߻�Ǵ� ray ��

        //�߰�
        public float FollowRadius { get; set; } = 25f; // �÷��̾� �߰� ���� �߰��ϴ� ����
        public Vector3 BackPosition { get; set; } //�����صξ��ٰ� �߰� ����� �����ϴ� ��ġ


        //����
        public float CombatRadius { get; set; } = 5f; // �÷��̾ �����ϴ� ����
        public bool CanAtk { get; set; } //���� ���� ����
        public bool IsHit { get; set; }  // ������ ���� �ߴ��� ����

        public float TotalAtkRate { get; set; } = 0; // ���� �ִϸ��̼� ��� �ð� + ���ݰ� ��� �ð�
        public float AttackDelay { get; set; } = 0f; // TotalAtkRate���� �����ϸ� CanAtk�� Ȱ��ȭ ��Ű�� ��


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

        public void InitStat() // ����,�⺻�� �ʱ�ȭ
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
            //���� ��Ÿ�� ���
            if(TotalAtkRate >= AttackDelay)
            {
                AttackDelay += Time.deltaTime;
                if (CanAtk) CanAtk = false;
            }
            else
            {
                if (!CanAtk) CanAtk = true;
            }
            //FSM ����
            StateContext.OnStateUpdate();
        }

        public void ChangeState(EnemyState state) // State ��ȯ
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

        public float GetAniLength(string clip_name) // �ִϸ��̼� speed�� ������ �ִϸ��̼� �� ����ð�
        {
            RuntimeAnimatorController controller = Animator.runtimeAnimatorController;
            float length=1.0f;
            
            foreach (AnimationClip clip in controller.animationClips) // clip_name�� length ���� ��ȯ
            {
                if (clip.name == clip_name)
                {
                    length = clip.length;
                    break;
                }
            }

            AnimatorStateInfo state_info = Animator.GetCurrentAnimatorStateInfo(0); // ���� ������� ������ ���� ��������
            float speed=1.0f;

            if (state_info.IsName(clip_name))
            {
                speed = state_info.speed * state_info.speedMultiplier;
            }
            else
            {
                Debug.Log($"���� ���°� '{clip_name}'�� ��ġ���� �ʽ��ϴ�.");
            }

            //Debug.Log($"{clip_name}�� ���� : {length}");
            //Debug.Log($"{clip_name}�� �ӵ� : {speed}");

            float real_length = length / speed;

            return real_length;
        }



        public void GetDamage(float damage)//�ǰݽ� ȣ��
        {
            (m_enemy_get_damage_state as EnemyGetDamageState).Damage = damage;
            ChangeState(EnemyState.GETDAMAGE);
        }

        public void DetectPlayer() // RayCast�� ����� �÷��̾� Ž�� 
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
                        Debug.Log("�÷��̾� ����");
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
