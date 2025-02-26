using UnityEngine;

public class TitleSceneCtrl : MonoBehaviour
{
    public void BTN_NewGame()
    {
        LoadingManager.Instance.LoadScene("Jongmin");
    }

    public void BTN_LoadGame()
    {

    }

    public void BTN_ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
