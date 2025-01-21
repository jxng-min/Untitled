using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private IState<PlayerCtrl> m_idle_state;
    private IState<PlayerCtrl> m_walk_state;
    private IState<PlayerCtrl> m_run_state; 

    public Rigidbody Rigidbody { get; private set; }
    public Transform Model { get; private set; }
    public Transform CameraArm { get; private set; }
    public Animator Animator { get; private set; }
    public Vector3 Direction { get; set; }
    public bool IsGround { get; set; }
    public PlayerStateContext StateContext { get; set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Model = GameObject.Find("PlayerModel").GetComponent<Transform>();
        CameraArm = GameObject.Find("CameraArm").GetComponent<Transform>();
        Animator = Model.GetComponent<Animator>();
    
        StateContext = new PlayerStateContext(this);

        m_idle_state = gameObject.AddComponent<PlayerIdleState>();
        m_walk_state = gameObject.AddComponent<PlayerWalkState>();
        m_run_state = gameObject.AddComponent<PlayerRunState>();

        StateContext.Transition(m_idle_state);
    }

    private void Update()
    {
        Direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        CheckGround();

        if(!IsGround)
        {
            if(Direction.magnitude > 0f)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    StateContext.Transition(m_run_state);
                }
                else
                {
                    StateContext.Transition(m_walk_state);
                }
            }
            else
            {
                StateContext.Transition(m_idle_state);
            }
        }

        StateContext.ExecuteUpdate();
    }

    private void CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.red);

        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit_info, 0.5f))
        {
            IsGround = true;
        }
        else
        {
            IsGround = false;
        }
    }
}
