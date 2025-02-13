using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.Rendering;

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
        protected IEnemyState<EnemyCtrl> m_enemy_attack_state;

        public EnemyStateContext StateContext { get; private set; }

        //�÷��̾� ������Ʈ ���ٿ�
        public GameObject Player;

        //Enemy ����
        [SerializeField]
        private EnemyStat m_origin_enemy_stat;
        public EnemyStat OriginEnemyStat { get {return m_origin_enemy_stat; } set { m_origin_enemy_stat=value; } }

        [SerializeField]
        private EnemyStat m_enemy_stat;
        public EnemyStat EnemyStat { get { return m_enemy_stat; } private set { m_enemy_stat = value; } }

        [SerializeField]
        private EnemySpawnData m_enemy_spawn_data;
        public EnemySpawnData EnemySpawnData { get { return m_enemy_spawn_data;}set{ m_enemy_spawn_data = value; } }

        //�ִϸ�����
        public Animator Animator { get; private set; }
        
        //Nav
        public NavMeshAgent Agent { get; private set; }

        //����
        public float PatrolRange { get; set; } = 10f;

        //�߰�
        public Vector3 BackPosition { get; set; } //�����صξ��ٰ� �߰� ����� �����ϴ� ��ġ

        //����
        public bool CanAtk { get; set; } //���� ���� ����
        public bool IsHit { get; set; }  // ������ ���� �ߴ��� ����
        public float TotalAtkRate { get; set; } = 0; // ���� �ִϸ��̼� ��� �ð� + ���ݰ� ��� �ð�
        public float AttackDelay { get; set; } = 0f; // TotalAtkRate���� �����ϸ� CanAtk�� Ȱ��ȭ ��Ű�� ��


        public GameObject[] m_drop_item_prefabs;

        public IObjectPool<EnemyCtrl> ManagedPool{ get; set; }

        private void OnEnable()
        {

        }

        public virtual void InitComponent()
        {
            m_enemy_idle_state = gameObject.AddComponent<EnemyIdleState>();
            m_enemy_attack_state = gameObject.AddComponent<EnemyAttackState>();
            m_enemy_back_state = gameObject.AddComponent<EnemyBackState>();
            m_enemy_dead_state = gameObject.AddComponent<EnemyDeadState>();
            m_enemy_found_player_state = gameObject.AddComponent<EnemyFoundPlayerState>();
            m_enemy_follow_state = gameObject.AddComponent<EnemyFollowState>();
            m_enemy_get_damage_state = gameObject.AddComponent<EnemyGetDamageState>();
            m_enemy_patrol_state = gameObject.AddComponent<EnemyPatrolState>();
            m_enemy_ready_state = gameObject.AddComponent<EnemyReadyState>();

            StateContext = new EnemyStateContext(this);

            Player = GameObject.Find("Player");

            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();           
        }

        public void InitStat() // ����,�⺻�� �ʱ�ȭ
        {
            EnemyStat = ScriptableObject.CreateInstance<EnemyStat>();
            EnemyStat.HP = OriginEnemyStat.HP;
            EnemyStat.AtkDamege = OriginEnemyStat.AtkDamege;
            EnemyStat.AtkRate = OriginEnemyStat.AtkRate;
            EnemyStat.MoveSpeed = OriginEnemyStat.MoveSpeed;
            EnemyStat.AtkRange = OriginEnemyStat.AtkRange;
            EnemyStat.FollowRange = OriginEnemyStat.FollowRange;
            EnemyStat.DetectRange= OriginEnemyStat.DetectRange;

            EnemySpawnData = ScriptableObject.CreateInstance<EnemySpawnData>();

            CanAtk = true;
            IsHit = false;
            Agent.speed = EnemyStat.MoveSpeed;
        }

        public void SetEnemyPool(IObjectPool<EnemyCtrl> pool) 
        {
            Debug.Log("Enemy풀 초기화");
            ManagedPool = pool;
        }

        public virtual void ReturnToPool()
        {
            Debug.Log($"{this.name} 반환");
            ManagedPool.Release(this);
        }

        void FixedUpdate()
        {
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

            //Debug.Log($"{clip_name}�� ���� : {length}");
            //Debug.Log($"{clip_name}�� �ӵ� : {speed}");

            float real_length = length / speed;

            return real_length;
        }
        public void UpdateHP(float value)//�ǰݽ� ȣ��
        {
            (m_enemy_get_damage_state as EnemyGetDamageState).Damage = value;
            ChangeState(EnemyState.GETDAMAGE);
        }

        public void DetectPlayer() // RayCast�� ����� �÷��̾� Ž�� 
        {
            if (Player.GetComponent<PlayerCtrl>().StateContext.Current is PlayerDeadState)
            {
                return;
            }

            int ray_count = (int)EnemyStat.DetectRange * 2;
            Vector3 y_offset = new Vector3(0, 2.0f, 0);
            float detect_angle = 45f;


            float start_angle = -detect_angle;
            float offset_angle = detect_angle / ray_count;

            for(int i =0; i< ray_count; i++)
            {
                float angle = start_angle + offset_angle * i;
                Vector3 dir = Quaternion.Euler(0,angle, 0) * transform.forward;

                Ray ray = new Ray(transform.position + y_offset, dir);
                if (Physics.Raycast(ray, out RaycastHit hit, EnemyStat.DetectRange))
                {
                    if(hit.collider.CompareTag("Player"))
                    {
                        if(EnemySpawnData.EnemyType == EnemyType.Axe)
                        {
                            ChangeState(EnemyState.FOUNDPLAYER);
                            Debug.DrawRay(transform.position + y_offset, dir * EnemyStat.DetectRange, Color.red);
                        }
                        else
                        {
                            ChangeState(EnemyState.FOLLOW);
                            Debug.DrawRay(transform.position + y_offset, dir * EnemyStat.DetectRange, Color.red);
                        }                                       
                    }                                                        
                }
                else
                {
                    Debug.DrawRay(transform.position + y_offset, dir * EnemyStat.DetectRange, Color.green);
                }
            }
        }
    }
}
