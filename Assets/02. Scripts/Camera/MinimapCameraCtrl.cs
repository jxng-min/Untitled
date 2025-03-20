using UnityEngine;
using Junyoung;

public class MinimapCameraCtrl : MonoBehaviour
{
    [SerializeField] private Transform m_player_icon;

    [Header("Mini Map Camera's Component")]
    [SerializeField] private Vector3 m_camera_size;

    [Header("Mini Map Camera's Constraint")]
    [SerializeField] private Vector3 m_region_center;
    [SerializeField] private Vector3 m_region_size;

    public Transform Minimap { get; set; }

    private void Awake()
    {
        Minimap = GameObject.Find("Minimap Camera").GetComponent<Transform>();
    }

    public void Update()
    {
        Minimap.rotation = Quaternion.Euler(90f, GameManager.Instance.Player.CameraArm.rotation.eulerAngles.y, 0f);
        m_player_icon.transform.rotation = Quaternion.Euler(90f, GameManager.Instance.Player.CameraArm.rotation.eulerAngles.y, 0f);

        Vector3 final_position = new Vector3(0f, 100f, 0f);
        final_position.x = Mathf.Clamp(GameManager.Instance.Player.transform.position.x, 25f, 175f);
        final_position.z = Mathf.Clamp(GameManager.Instance.Player.transform.position.z, 25f, 175f);

        transform.position = final_position;
    }
}