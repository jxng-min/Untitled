using UnityEngine;

public class MinimapCameraCtrl : MonoBehaviour
{
    [Header("Player's Component")]
    private PlayerCtrl m_player_ctrl;
    [SerializeField] private Transform m_player_icon;

    [Header("Mini Map Camera's Component")]
    [SerializeField] private Vector3 m_camera_size;

    [Header("Mini Map Camera's Constraint")]
    [SerializeField] private Vector3 m_region_center;
    [SerializeField] private Vector3 m_region_size;

    public Transform CameraArm { get; set; }
    public Transform Minimap { get; set; }

    private void Awake()
    {
        m_player_ctrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        CameraArm = GameObject.Find("CameraArm").GetComponent<Transform>();
        Minimap = GameObject.Find("Minimap Camera").GetComponent<Transform>();
    }

    public void Update()
    {
        Minimap.rotation = Quaternion.Euler(90f, CameraArm.rotation.eulerAngles.y, 0f);
        m_player_icon.transform.rotation = Quaternion.Euler(90f, CameraArm.rotation.eulerAngles.y, 0f);

        Vector3 final_position = new Vector3(0f, 100f, 0f);
        final_position.x = Mathf.Clamp(m_player_ctrl.transform.position.x, 25f, 175f);
        final_position.z = Mathf.Clamp(m_player_ctrl.transform.position.z, 25f, 175f);

        transform.position = final_position;
    }
}