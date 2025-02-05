using UnityEngine;
using UnityEngine.UI;

public class MinimapCameraCtrl : MonoBehaviour
{
    public Transform CameraArm { get; set; }
    public Transform Minimap { get; set; }

    private void Awake()
    {
        CameraArm = GameObject.Find("CameraArm").GetComponent<Transform>();
        Minimap = GameObject.Find("Minimap Camera").GetComponent<Transform>();
    }

    public void Update()
    {
        Minimap.rotation = Quaternion.Euler(90f, CameraArm.rotation.eulerAngles.y, 0f);
    }
}
