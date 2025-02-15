using System;
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
    private IState<PlayerCtrl> m_dead_state;
    private IState<PlayerCtrl> m_skill1_state;
    private IState<PlayerCtrl> m_skill2_state;
    private IState<PlayerCtrl> m_skill3_state;
    #endregion

    [SerializeField] private GameObject m_skill1_prefab;
    [SerializeField] private GameObject m_skill2_prefab;
    [SerializeField] private GameObject m_skill3_prefab;

    #region Properties
    public Transform Model { get; private set; }
    public Transform CameraArm { get; private set; }
    public CameraShaker Camera { get; set; }
    public Animator Animator { get; private set; }

    [Header("Move Component")]
    public Rigidbody Rigidbody { get; private set; }
    public Vector3 Direction { get; set; }    

    [Header("Jump Component")]
    public float FallTime { get; set; }
    public float JumpPower { get; set; } = 7f;
    public bool IsGround { get; set; }

    [Header("Attack Component")]
    public WeaponCtrl Weapon { get; set; }
    public bool IsAttack { get; set; }
    public GameObject Skill1Effect { get { return m_skill1_prefab; } }
    public GameObject Skill2Effect { get { return m_skill2_prefab; } }
    public GameObject Skill3Effect { get { return m_skill3_prefab; } }

    [Header("Block Component")]
    public bool IsBlock { get; set; }
    public float BlockTime { get; set; }

    [Header("Skill Component")]
    public bool Skill1Ready { get; set; }
    public float Skill1Time { get; set; }
    public float Skill1CoolTime { get; set; } = 10f;
    public bool Skill2Ready { get; set; }
    public float Skill2Time { get; set; }
    public float Skill2CoolTime { get; set; } = 20f;
    public bool Skill3Ready { get; set; }
    public float Skill3Time { get; set; }
    public float Skill3CoolTime { get; set; } = 7f;

    [Header("State Component")]
    public PlayerStateContext StateContext { get; set; }

    [Header("장비 인벤토리")]
    [SerializeField] private EquipmentInventory m_equipment_inventory;
    public EquipmentInventory Equipment 
    { 
        get { return m_equipment_inventory; }
    }

    #endregion

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Model = GameObject.Find("PlayerModel").GetComponent<Transform>();
        CameraArm = GameObject.Find("CameraArm").GetComponent<Transform>();
        Camera = CameraArm.GetComponentInChildren<CameraShaker>();
        Animator = Model.GetComponent<Animator>();
        Weapon = FindAnyObjectByType<WeaponCtrl>();
    
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
        m_dead_state = gameObject.AddComponent<PlayerDeadState>();
        m_skill1_state = gameObject.AddComponent<PlayerSkill1State>();
        m_skill2_state = gameObject.AddComponent<PlayerSkill2State>();
        m_skill3_state = gameObject.AddComponent<PlayerSkill3State>();

        ChangeState(PlayerState.IDLE);
    }

    private void Start()
    {
        transform.position = DataManager.Instance.Data.Position;

        UpdateAttackSpeed();

        Skill1Ready = true;
        Skill2Ready = true;
        Skill3Ready = true;
    }

    private void Update()
    {
        DataManager.Instance.Data.Position = transform.position;

        if(!InventoryMain.Active && !EquipmentInventory.Active && !StatInventory.Active && !ConversationManager.Instance.IsTalking)
        {
            Direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        }
        
        CheckGround();
        CheckFalling();
        CheckBlocking();

        CheckSkill1();
        CheckSkill2();
        CheckSkill3();

        if(!InventoryMain.Active && !EquipmentInventory.Active && !StatInventory.Active && !ConversationManager.Instance.IsTalking)
        {
            StateContext.ExecuteUpdate();
        }
        
    }

    public void UpdateAttackSpeed()
    {
        Animator.SetFloat("AttackSpeed1", 1.5f / DataManager.Instance.GetMaxStat().Rate);
        Animator.SetFloat("AttackSpeed2", 1.67f / DataManager.Instance.GetMaxStat().Rate);
        Animator.SetFloat("AttackSpeed3", 1.28f / DataManager.Instance.GetMaxStat().Rate);
    }

    public void Move(float speed)
    {
        Vector3 forward_direction = new Vector3(CameraArm.forward.x, 0f, CameraArm.forward.z);
        Vector3 right_direction = new Vector3(CameraArm.right.x, 0f, CameraArm.right.z);

        Vector3 final_direction = ((forward_direction * Direction.z) + (right_direction * Direction.x)).normalized;

        Vector3 velocity = final_direction * speed;

        Model.forward = Vector3.Lerp(forward_direction, Model.forward, Time.deltaTime);

        Vector3 new_position = Rigidbody.position + velocity * Time.deltaTime;
        Rigidbody.MovePosition(new_position);
    }

    public void Jump(float power)
        {
            if(IsGround && Input.GetButtonDown("Jump"))
            {
                if(InventoryMain.Active || EquipmentInventory.Active || StatInventory.Active || ConversationManager.Instance.IsTalking)
                {
                    return;
                }
                
                Rigidbody.linearVelocity = new Vector3(Rigidbody.linearVelocity.x, 0, Rigidbody.linearVelocity.z);
                Rigidbody.AddForce(Vector3.up * power, ForceMode.Impulse);
                ChangeState(PlayerState.JUMPIN);
            }
        }

    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && IsGround)
        {
            if(InventoryMain.Active || EquipmentInventory.Active || StatInventory.Active || ConversationManager.Instance.IsTalking)
            {
                return;
            }

            ChangeState(PlayerState.ATTACK);
        }
    }

    public void Skill1()
    {
        if(InventoryMain.Active || EquipmentInventory.Active || StatInventory.Active || ConversationManager.Instance.IsTalking)
        {
            return;
        }

        if(DataManager.Instance.Data.Stat.MP < 6f)
        {
            return;
        }

        if(Skill1Ready && !IsAttack && Input.GetKeyDown(KeyCode.Alpha1) && IsGround)
        {
            ChangeState(PlayerState.SKILL1);
        }
    }

    public void Skill2()
    {
        if(InventoryMain.Active || EquipmentInventory.Active || StatInventory.Active || ConversationManager.Instance.IsTalking)
        {
            return;
        }

        if(DataManager.Instance.Data.Stat.MP < 1f)
        {
            return;
        }

        if(Skill2Ready && !IsAttack && Input.GetKeyDown(KeyCode.Alpha2) && IsGround)
        {
            ChangeState(PlayerState.SKILL2);
        }
    }

    public void Skill3()
    {
        if(InventoryMain.Active && EquipmentInventory.Active && StatInventory.Active && ConversationManager.Instance.IsTalking)
        {
            return;
        }

        if(DataManager.Instance.Data.Stat.MP < 3f)
        {
            return;
        }
        
        if(Skill3Ready && !IsAttack && Input.GetKeyDown(KeyCode.Alpha3) && IsGround)
        {
            ChangeState(PlayerState.SKILL3);
        }
    }

    public void UpdateHP(float value)
    {
        float final_value = value;

        var indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator).GetComponent<DamageIndicator>();

        if(final_value < 0f)
        {
            final_value += DataManager.Instance.Data.Stat.DEF;

            if(final_value >= 0f)
            {
                indicator.Init(transform.position + Vector3.up * 3f, final_value, Color.black, 0.5f, true);
                return;
            }

            if(IsBlock)
            {
                ChangeState(PlayerState.BLOCKDAMAGE);
                final_value = value * 0.2f;
            }
            else
            {
                ChangeState(PlayerState.DAMAGE);
            }

            Camera.Shaking(0.2f, 0.1f);
        }

        DataManager.Instance.Data.Stat.HP += final_value;

        indicator.Init(transform.position + Vector3.up * 3f, final_value, final_value < 0 ? Color.red : Color.green);

        DataManager.Instance.SavePlayerData();
    }

    public void UpdateMP(float value)
    {
        float final_value = value;

        DataManager.Instance.Data.Stat.MP += value;

        var indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator).GetComponent<DamageIndicator>();
        indicator.Init(transform.position + Vector3.up * 3f, final_value, final_value < 0 ? Color.magenta : Color.blue);

        DataManager.Instance.SavePlayerData();
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
            
            case PlayerState.DEAD:
                StateContext.Transition(m_dead_state);
                break;
            
            case PlayerState.SKILL1:
                StateContext.Transition(m_skill1_state);
                break;
            
            case PlayerState.SKILL2:
                StateContext.Transition(m_skill2_state);
                break;
            
            case PlayerState.SKILL3:
                StateContext.Transition(m_skill3_state);
                break;
        }
    }

    private void CheckGround()
    {
        RaycastHit[] hit_infos = new RaycastHit[4];
        Vector3[] ray_directions = new Vector3[4] 
        { 
            (Quaternion.Euler(45, 0, 0) * new Vector3(0, -transform.position.y, 0)).normalized,
            (Quaternion.Euler(-45, 0, 0) * new Vector3(0, -transform.position.y, 0)).normalized,
            (Quaternion.Euler(0, 0, 45) * new Vector3(0, -transform.position.y, 0)).normalized,
            (Quaternion.Euler(0, 0, -45) * new Vector3(0, -transform.position.y, 0)).normalized 
        };

        for(int i = 0; i < 4; i++)
        {
            
            if(Physics.Raycast(transform.position + Vector3.up * 0.2f, ray_directions[i], out hit_infos[i], 1.3f))
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.2f, ray_directions[i], Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.2f, ray_directions[i], Color.red);
            }
        }

        IsGround = false;
        for(int i = 0; i < 4; i++)
        {
            if(hit_infos[i].collider != null && !hit_infos[i].collider.CompareTag("Player"))
            {
                IsGround = true;                
            }
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

    private void CheckSkill1()
    {
        if(!Skill1Ready)
        {
            Skill1Time += Time.deltaTime;

            if(Skill1Time >= Skill1CoolTime)
            {
                Skill1Ready = true;
            }
        }
        else
        {
            Skill1Time = 0f;
        }
    }

    private void CheckSkill2()
    {
        if(!Skill2Ready)
        {
            Skill2Time += Time.deltaTime;

            if(Skill2Time >= Skill2CoolTime)
            {
                Skill2Ready = true;
            }
        }
        else
        {
            Skill2Time = 0f;
        }
    }

    private void CheckSkill3()
    {
        if(!Skill3Ready)
        {
            Skill3Time += Time.deltaTime;

            if(Skill3Time >= Skill3CoolTime)
            {
                Skill3Ready = true;
            }
        }
        else
        {
            Skill3Time = 0f;
        }
    }
}
