using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region State Variables
    private IState<PlayerCtrl> m_idle_state;
    private IState<PlayerCtrl> m_walk_state;
    private IState<PlayerCtrl> m_run_state;
    private IState<PlayerCtrl> m_jump_in_state;
    private IState<PlayerCtrl> m_jumping_state;
    private IState<PlayerCtrl> m_jump_out_state;
    private IState<PlayerCtrl> m_attack_state;
    private IState<PlayerCtrl> m_block_state;
    private IState<PlayerCtrl> m_damage_state;
    private IState<PlayerCtrl> m_block_damage_state;
    #endregion

    #region Properties
    public Transform Model { get; private set; }
    public Transform CameraArm { get; private set; }
    public Animator Animator { get; private set; }
    public DataManager Data { get; private set;}

    [Header("Move Component")]
    public Rigidbody Rigidbody { get; private set; }
    public Vector3 Direction { get; set; }    

    [Header("Jump Component")]
    public float FallTime { get; set; }
    public float JumpPower { get; set; } = 7f;
    public bool IsGround { get; set; }

    [Header("Attack Component")]
    public float AttackDelay { get; set; }
    public bool AttackReady { get; set; }
    public WeaponCtrl Weapon { get; set; }

    [Header("Block Component")]
    public bool IsBlock { get; set; }
    public float BlockTime { get; set; }

    [Header("State Component")]
    public PlayerStateContext StateContext { get; set; }
    #endregion

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Model = GameObject.Find("PlayerModel").GetComponent<Transform>();
        CameraArm = GameObject.Find("CameraArm").GetComponent<Transform>();
        Animator = Model.GetComponent<Animator>();
        Weapon = FindAnyObjectByType<WeaponCtrl>();
        Data =  GameObject.Find("DataManager").GetComponent<DataManager>();
        
    
        StateContext = new PlayerStateContext(this);

        m_idle_state = gameObject.AddComponent<PlayerIdleState>();
        m_walk_state = gameObject.AddComponent<PlayerWalkState>();
        m_run_state = gameObject.AddComponent<PlayerRunState>();
        m_jump_in_state = gameObject.AddComponent<PlayerJumpInState>();
        m_jumping_state = gameObject.AddComponent<PlayerJumpingState>();
        m_jump_out_state = gameObject.AddComponent<PlayerJumpOutState>();
        m_attack_state = gameObject.AddComponent<PlayerAttackState>();
        m_block_state = gameObject.AddComponent<PlayerBlockState>();
        m_damage_state = gameObject.AddComponent<PlayerDamageState>();
        m_block_damage_state = gameObject.AddComponent<PlayerBlockDamageState>();

        ChangeState(PlayerState.IDLE);
    }

    private void Update()
    {
        Direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        CheckGround();
        CheckFalling();
        CheckBlocking();

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetDamage(10);
        }
    }

    private void FixedUpdate()
    {
        StateContext.ExecuteUpdate();
        //Debug.Log($"{StateContext.Current}");
    }

    public void Move(float speed)
    {
        Vector3 forward_direction = new Vector3(CameraArm.forward.x, 0f, CameraArm.forward.z);
        Vector3 right_direction = new Vector3(CameraArm.right.x, 0f, CameraArm.right.z);

        Vector3 final_direction = ((forward_direction * Direction.z) + (right_direction * Direction.x)).normalized;

        Vector3 velocity = final_direction * speed;

        Model.forward = Vector3.Lerp(forward_direction, Model.forward, Time.deltaTime * 15f);

        Vector3 new_position = Rigidbody.position + velocity * Time.deltaTime;
        Rigidbody.MovePosition(new_position);
    }

    public void Jump(float power)
    {
        if(IsGround && Input.GetButtonDown("Jump"))
        {
            Rigidbody.AddForce(Vector3.up * power, ForceMode.Impulse);
            ChangeState(PlayerState.JUMPIN);
        }
    }

    public void Attack()
    {
        AttackDelay += Time.deltaTime;
        AttackReady = AttackDelay > Weapon.Info.Rate;

        if(AttackReady && Input.GetKeyDown(KeyCode.Mouse0) && IsGround)
        {
            Weapon.Use();
            ChangeState(PlayerState.ATTACK);
            AttackDelay = 0;
        }
    }

    public void GetDamage(float damage)
    {
        //
        if(IsBlock)
        {
            // 방어 이펙트가 있었으면 함.
            Debug.Log($"{damage * 0.2f}의 피해를 입었다.");
            ChangeState(PlayerState.BLOCKDAMAGE);
            Data.PlayerStat.HP = damage * 0.2f;
        }
        else
        {
            Debug.Log($"{damage}의 피해를 입었다.");
            ChangeState(PlayerState.DAMAGE);
            Data.PlayerStat.HP = damage;
        }
    }

    public void ChangeState(PlayerState state)
    {
        switch(state)
        {
            case PlayerState.IDLE:
                StateContext.Transition(m_idle_state);
                break;
            
            case PlayerState.WALK:
                StateContext.Transition(m_walk_state);
                break;
            
            case PlayerState.RUN:
                StateContext.Transition(m_run_state);
                break;

            case PlayerState.JUMPIN:
                StateContext.Transition(m_jump_in_state);
                break;

            case PlayerState.JUMPING:
                StateContext.Transition(m_jumping_state);
                break;
            
            case PlayerState.JUMPOUT:
                StateContext.Transition(m_jump_out_state);
                break;
            
            case PlayerState.ATTACK:
                StateContext.Transition(m_attack_state);
                break;
            
            case PlayerState.BLOCK:
                StateContext.Transition(m_block_state);
                break;
            
            case PlayerState.BLOCKDAMAGE:
                StateContext.Transition(m_block_damage_state);
                break;

            case PlayerState.DAMAGE:
                StateContext.Transition(m_damage_state);
                break;
        }
    }

    private void CheckGround()
    {
        if(Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit_info, 0.4f))
        {
            if(hit_info.collider != null && !hit_info.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.2f, Vector3.down * 0.4f, Color.green);
                IsGround = true;
            }

        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.2f, Vector3.down * 0.4f, Color.red);
            IsGround = false;
        }
    }

    private void CheckFalling()
    {
        if(IsGround)
        {
            FallTime = 0f;
        }
        else
        {
            FallTime += Time.deltaTime;
        }
    }

    private void CheckBlocking()
    {
        if(IsBlock)
        {
            BlockTime += Time.deltaTime;
        }
        else
        {
            BlockTime = 0f;
        }
    }
}
