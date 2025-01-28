using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_camera;
    private float m_camera_distance;

    public Vector2 Delta { get; set; }
    public bool Inversal { get; set; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        m_camera_distance = Vector3.Distance(m_player.position, m_camera.position);
    }

    private void Update()
    {
        Rotation();
        Translation();
    }

    private void Rotation()
    {
        Delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        Vector3 direction = transform.rotation.eulerAngles;
        float final_x = Inversal ? (direction.x - Delta.y) : (direction.x + Delta.y);
        if(final_x < 180f)
        {
            final_x = Mathf.Clamp(final_x, -1f, 60f);
        }
        else
        {
            final_x = Mathf.Clamp(final_x, 340f, 361f);
        }

        transform.rotation = Quaternion.Euler(final_x, direction.y + Delta.x, direction.z);
    }

    private void Translation()
    {
        Vector3 ray_direction = (m_camera.position - m_player.position).normalized;

        Debug.DrawRay(m_player.position, ray_direction * m_camera_distance, Color.red);
        if(Physics.Raycast(m_player.position, ray_direction, out RaycastHit ray_info, m_camera_distance))
        {
            if(!ray_info.collider.CompareTag("Player") && !ray_info.collider.CompareTag("Enemy") && !ray_info.collider.CompareTag("Weapon") &&  ray_info.collider != null)
            {
                m_camera.position = Vector3.Lerp(m_camera.position, ray_info.point - ray_direction * 0.3f, Time.deltaTime * 10f);
                return;
            }
        }
        
        m_camera.position = Vector3.Lerp(m_camera.position, m_player.position + ray_direction * m_camera_distance, Time.deltaTime * 10f);
    }
}