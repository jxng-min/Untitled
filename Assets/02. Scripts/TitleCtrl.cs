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
        SoundManager.Instance.PlayEffect("Button Click");
        LoadingManager.Instance.LoadScene("Jongmin");
    }

    public void BTN_LoadGame()
    {
        SoundManager.Instance.PlayEffect("Button Click");
    }

    public void BTN_Exit()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
