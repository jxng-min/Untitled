using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingManager : Singleton<SettingManager>
{
    private static bool is_ui_active = false;
    public static bool IsActive
    {
        get { return is_ui_active; }
        set { is_ui_active = value; }
    }

    private string m_setting_data_path;

    [Header("환경설정 UI")]
    [SerializeField] private GameObject m_setting_ui_object;
    [Header("오버레이 UI들이 위치하는 캔버스")]
    [SerializeField] private GameObject m_canvas;

    [Header("사운드 설정 UI 관련")]
    [SerializeField] private Toggle m_sound_panel_toggle;
    [SerializeField] private GameObject m_sound_panel;

    [Header("게임 설정 UI 관련")]
    [SerializeField] private Toggle m_game_panel_toggle;
    [SerializeField] private GameObject m_game_panel;

    #region 사운드 관련
    [Space(50)]
    [Header("배경음 제어")]
    [SerializeField] Toggle m_background_sound_toggle;
    [SerializeField] Slider m_bacgkround_sound_slider;

    [Header("효과음 제어")]
    [SerializeField] Toggle m_effect_sound_toggle;
    [SerializeField] Slider m_effect_sound_slider;
    #endregion

    #region 게임 관련
    [Space(50)]
    [Header("카메라 흔들림 제어")]
    [SerializeField] private CameraShaker m_camera_shaker;
    [SerializeField] private Toggle m_camera_shaker_toggle;

    [Header("전처리 볼륨 제어")]
    [SerializeField] private Camera m_shader_camera;
    [SerializeField] private Toggle m_shader_toggle;

    [Header("디스플레이 비율 제어")]
    [SerializeField] private TMP_Dropdown m_resolution_drop_down;
    private Resolution[] m_resolutions;

    [Header("타이틀 이동 버튼")]
    [SerializeField] private Button m_go_to_title_button;

    [Header("게임 종료 버튼")]
    [SerializeField] private Button m_game_exit_button;
    #endregion

    private SettingData m_setting_data = new SettingData();
    public SettingData Setting
    {
        get { return m_setting_data; }
        set { m_setting_data = value; }
    }

    private new void Awake()
    {
        base.Awake();

        m_setting_data_path = Path.Combine(Application.persistentDataPath, "SettingData.json");

        Init();
    }

    private void Start()
    {
        m_setting_ui_object.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!IsActive)
            {
                IsActive = true;
                m_setting_ui_object.SetActive(true);
                GetResolutions();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                IsActive = false;
                m_setting_ui_object.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private void Init()
    {
        if(File.Exists(m_setting_data_path))
        {
            LoadSettingData();
        }
        else
        {
            m_background_sound_toggle.isOn = true;
            m_bacgkround_sound_slider.value = 0.5f;

            m_effect_sound_toggle.isOn = true;
            m_effect_sound_slider.value = 0.5f;

            m_camera_shaker_toggle.isOn = true;
            m_shader_toggle.isOn = true;

            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
    }

    private void LoadSettingData()
    {
        var json_data = File.ReadAllText(m_setting_data_path);
        Setting = JsonUtility.FromJson<SettingData>(json_data);

        m_background_sound_toggle.isOn = Setting.BackgroundActive;
        m_bacgkround_sound_slider.value = Setting.Backgroundvalue;

        m_effect_sound_toggle.isOn = Setting.EffectActive;
        m_effect_sound_slider.value = Setting.EffectValue;

        m_camera_shaker_toggle.isOn = Setting.CameraShakerActive;
        m_shader_toggle.isOn = Setting.VolumeActive;

        Screen.SetResolution((int)Setting.Resolution.x, (int)Setting.Resolution.y, Screen.fullScreen);
    }

    private void SaveSettingData()
    {
        Setting.BackgroundActive = m_background_sound_toggle.isOn;
        Setting.Backgroundvalue = m_bacgkround_sound_slider.value;

        Setting.EffectActive = m_effect_sound_toggle.isOn;
        Setting.EffectValue = m_effect_sound_slider.value;

        Setting.CameraShakerActive = m_camera_shaker_toggle.isOn;
        Setting.VolumeActive = m_shader_toggle.isOn;

        Setting.Resolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

        var json_data = JsonUtility.ToJson(Setting);
        File.WriteAllText(m_setting_data_path, json_data);
    }

    public void Toggle_CameraShakingControl()
    {
        m_camera_shaker.IsEnable = m_camera_shaker_toggle.isOn;
    }

    public void Toggle_PostVolumeControl()
    {
        m_shader_camera.GetComponent<Volume>().enabled = m_shader_toggle.isOn;
    }

    private void GetResolutions()
    {
        m_resolutions = Screen.resolutions;

        m_resolution_drop_down.ClearOptions();

        HashSet<string> options = new HashSet<string>();

        int current_index = 0;
        for(int i = 0; i < m_resolutions.Length; i++)
        {
            string option = m_resolutions[i].width + "x" + m_resolutions[i].height;
            options.Add(option);

            if(m_resolutions[i].width == Screen.currentResolution.width && m_resolutions[i].height == Screen.currentResolution.height)
            {
                current_index = i;
            }
        }

        m_resolution_drop_down.AddOptions(new List<string>(options));
        m_resolution_drop_down.value = current_index;
        m_resolution_drop_down.RefreshShownValue();
    }

    public void DropDown_SetResolution(int index)
    {
        Resolution resolution = m_resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void BTN_GoToTitle()
    {
        QuestManager.Instance.SaveCurrentQuests();
        ItemShopManager.Instance.SaveData();
        DataManager.Instance.SaveInventory();
        DataManager.Instance.SavePlayerData();
        SaveSettingData();

        IsActive = false;
        m_setting_ui_object.SetActive(false);
        m_canvas.SetActive(false);

        LoadingManager.Instance.LoadScene("Title");
    }

    public void BTN_GameExit()
    {
        QuestManager.Instance.SaveCurrentQuests();
        ItemShopManager.Instance.SaveData();
        DataManager.Instance.SaveInventory();
        DataManager.Instance.SavePlayerData();
        SaveSettingData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Toggle_BackgroundSoundToggle()
    {
        m_bacgkround_sound_slider.enabled = m_background_sound_toggle.isOn;
    }

    public void Slider_BackgroundSoundControl()
    {
        // 배경음 크기 조절
    }

    public void Toggle_EffectSoundToggle()
    {
        m_effect_sound_slider.enabled = m_effect_sound_toggle.isOn;
    }

    public void Slider_EffectSoundControl()
    {
        // 효과음 크기 조절
    }

}
