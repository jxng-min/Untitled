using UnityEngine;

public class TitleSceneCtrl : MonoBehaviour
{
    public void BTN_NewGame()
    {
        LoadingManager.Instance.LoadScene("Jongmin");
    }
}
