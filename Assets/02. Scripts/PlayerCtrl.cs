using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] private Transform m_player_model;

    [Header("Camera")]
    [SerializeField] private Transform m_camera_arm;

    [Header("Moving Property")]

    [SerializeField] private float m_walk_speed;
    [SerializeField] private float m_run_speed;

    [Header("Jumping Property")]
    [SerializeField] private float m_jump_power;

    private PlayerStateContext m_state_context;
    private IState<PlayerCtrl> m_idle_state;
    private IState<PlayerCtrl> m_walk_state;
    private IState<PlayerCtrl> m_run_state;

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; set;}
    public Transform CameraArm { get; private set;}
    public Transform Model { get; private set; }
    public bool IsGround { get; private set; }
    public Vector3 Direction { get; set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = m_player_model.GetComponent<Animator>();

        m_state_context = new PlayerStateContext(this);

        m_idle_state = gameObject.AddComponent<PlayerIdle>();
        m_walk_state = gameObject.AddComponent<PlayerWalk>();
        m_run_state = gameObject.AddComponent<PlayerRun>();

        ChangeState(PlayerState.IDLE);
    }

    private void Update()
    {
        Direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        CheckGround();
    }

    private void FixedUpdate()
    {
        if(IsGround)
        {
            if(Input.GetButton("Jump"))
                ChangeState(PlayerState.JUMP_UP);

            if(Direction.magnitude > 0f)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    ChangeState(PlayerState.RUN);
                }
                else
                {
                    ChangeState(PlayerState.WALK);
                }
            }
            else
            {
                ChangeState(PlayerState.IDLE);
            }
            
        }

        m_state_context.State.ExecuteUpdate(this);
    }
    
    private void CheckGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.2f))
        {
            if(!hit.collider.CompareTag("Player") && hit.collider != null)
            {
                IsGround = true;
                return;
            }
        }
        else
        {
            IsGround = false;
        }
    }

    public void ChangeState(PlayerState state)
    {
        switch(state)
        {
            case PlayerState.IDLE:
                m_state_context.Transition(m_idle_state);
                break;
            
            case PlayerState.WALK:
                m_state_context.Transition(m_walk_state);
                break;
            
            case PlayerState.RUN:
                m_state_context.Transition(m_run_state);
                break;
        }
    }
}