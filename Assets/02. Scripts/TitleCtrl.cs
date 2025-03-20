using Junyoung;
using UnityEngine;

public class TitleCtrl : MonoBehaviour
{
    private void Start()
    {
        EventBus.Publish(GameEventType.NONE);

        if(SettingManager.Instance.Setting.BackgroundActive)
        {
            SoundManager.Instance.PlayBGM("Title Background");
        }        
    }

    public void BTN_NewGame()
    {
        LoadingManager.Instance.LoadScene("Jongmin");
    }

    public void BTN_LoadGame()
    {

    }

    public void BTN_Exit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
