using Junyoung;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Setter : MonoBehaviour
{
    [Header("설정 UI 오브젝트")]
    [SerializeField] private GameObject m_setting_ui_object;

    [Header("소리 UI 오브젝트")]
    [SerializeField] private GameObject m_sound_ui_object;

    [Header("게임 UI 오브젝트")]
    [SerializeField] private GameObject m_game_ui_object;

    #region 소리 관련 설정
    [Space(30)]
    [Header("BGM 출력 토글")]
    [SerializeField] private Toggle m_background_on_toggle;

    [Header("BGM 제어 슬라이더")]
    [SerializeField] private Slider m_background_control_slider;
   
    [Header("Effect 출력 토글")]
    [SerializeField] private Toggle m_effect_on_toggle;
    
    [Header("Effect 제어 슬라이더")]
    [SerializeField] private Slider m_effect_control_slider;
    #endregion

    #region 게임 관련 설정
    [Space(30)]
    [Header("카메라 흔들림 제어")]
    [SerializeField] private Toggle m_camera_shaking_toggle;

    [Header("전역 쉐이더 제어")]
    [SerializeField] private Toggle m_global_volume_toggle;

    [Header("디스플레이 비율 드롭다운")]
    [SerializeField] private Dropdown m_display_aspect_ratio_dropdown;
    #endregion

    private void Awake()
    {
        m_background_on_toggle.isOn = SettingManager.Instance.Setting.BackgroundActive;
        m_background_control_slider.value = SettingManager.Instance.Setting.Backgroundvalue;

        m_effect_on_toggle.isOn = SettingManager.Instance.Setting.EffectActive;
        m_effect_control_slider.value = SettingManager.Instance.Setting.EffectValue;

        m_camera_shaking_toggle.isOn = SettingManager.Instance.Setting.CameraShakerActive;
        m_global_volume_toggle.isOn = SettingManager.Instance.Setting.VolumeActive;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.Instance.PlayEffect("Button Click");

            if(GameManager.Instance.GameState == GameEventType.PLAYING)
            {
                EventBus.Publish(GameEventType.SETTING);
                m_setting_ui_object.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if(GameManager.Instance.GameState == GameEventType.SETTING)
            {
                EventBus.Publish(GameEventType.PLAYING);
                m_setting_ui_object.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }   
    }

    public void BTN_SoundPanel()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        m_sound_ui_object.SetActive(true);
        m_game_ui_object.SetActive(false);
    }

    public void BTN_GamePanel()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        m_sound_ui_object.SetActive(false);
        m_game_ui_object.SetActive(true);
    }

    public void Toggle_BGMPrint()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Setting.BackgroundActive = m_background_on_toggle.isOn;

        if(SettingManager.Instance.Setting.BackgroundActive is false)
        {
            SoundManager.Instance.BGM.Pause();
        }
        else
        {
            SoundManager.Instance.BGM.UnPause();
        }
    }

    public void Slider_BGMSlider()
    {
        SettingManager.Instance.Setting.Backgroundvalue = m_background_control_slider.value;
        SoundManager.Instance.BGM.volume = SettingManager.Instance.Setting.Backgroundvalue;
    }

    public void Toggle_EffectPrint()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Setting.EffectActive = m_effect_on_toggle.isOn;
    }

    public void Slider_EffectSlider()
    {
        SettingManager.Instance.Setting.EffectValue = m_effect_control_slider.value;
    }

    public void Toggle_CameraShaking()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Setting.CameraShakerActive = m_camera_shaking_toggle.isOn;
        Camera.main.GetComponent<CameraShaker>().enabled = SettingManager.Instance.Setting.CameraShakerActive;
    }

    public void Toggle_GlobalVolume()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Setting.VolumeActive = m_global_volume_toggle.isOn;
        Camera.main.GetComponent<Volume>().enabled = SettingManager.Instance.Setting.VolumeActive;
    }

    public void BTN_Title()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.SaveSettingData();
        DataManager.Instance.SaveInventory();
        DataManager.Instance.SavePlayerData();

        LoadingManager.Instance.LoadScene("Title");
    }

    public void BTN_Exit()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.SaveSettingData();
        DataManager.Instance.SaveInventory();
        DataManager.Instance.SavePlayerData();

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
