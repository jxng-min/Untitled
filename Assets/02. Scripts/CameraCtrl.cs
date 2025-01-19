using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform m_camera;

    [Header("Camera Info")]
    private float m_ray_distance;
    [SerializeField] private float m_camera_fix;

    public bool Reversal { get; set;}

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        m_ray_distance = (m_camera.position - transform.position).magnitude;
    }

    private void Update()   
    {
        CameraRotation();
        CameraTranslation();
    }

    private void CameraRotation()
    {
        float mouse_delta_x = Input.GetAxisRaw("Mouse X");
        float mouse_delta_y = Input.GetAxisRaw("Mouse Y");

        Vector3 direction = transform.rotation.eulerAngles;

        float clamped_x_rotation = Reversal ? (direction.x + mouse_delta_y) : (direction.x - mouse_delta_y);
        if(clamped_x_rotation < 180f)
        {
            clamped_x_rotation = Mathf.Clamp(clamped_x_rotation, -1f, 45f);
        }
        else
        {
            clamped_x_rotation = Mathf.Clamp(clamped_x_rotation, 340f, 361f);
        }

        transform.rotation = Quaternion.Euler(clamped_x_rotation, direction.y + mouse_delta_x, direction.z);
    }

    private void CameraTranslation()
    {
        Vector3 ray_direction = m_camera.position - transform.position;

        if(Physics.Raycast(transform.position, ray_direction, out RaycastHit hit, m_ray_distance) && hit.collider.tag != "Player")
        {
            m_camera.position = Vector3.Lerp(m_camera.position, hit.point - ray_direction.normalized * m_camera_fix, Time.deltaTime * 20f);
        }
        else
        {
            m_camera.position = Vector3.Lerp(m_camera.position, ray_direction.normalized * m_ray_distance + transform.position, Time.deltaTime * 20f);
        }
    }
}