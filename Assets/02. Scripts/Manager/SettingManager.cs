using System.IO;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    private string m_setting_data_path;

    private SettingData m_setting_data;
    public SettingData Setting
    {
        get { return m_setting_data; }
        set { m_setting_data = value; }
    }

    private new void Awake()
    {
        base.Awake();

        m_setting_data_path = Path.Combine(Application.persistentDataPath, "SettingData.json");
        m_setting_data = new SettingData();
    }

    public void Initialize()
    {
        if(File.Exists(m_setting_data_path))
        {
            LoadSettingData();
        }
        else
        {
            Setting.BackgroundActive = true;
            Setting.Backgroundvalue = 0.5f;

            Setting.EffectActive = true;
            Setting.EffectValue = 0.5f;

            Setting.CameraShakerActive = true;
            Setting.VolumeActive =  true;

            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
    }

    public void LoadSettingData()
    {
        var json_data = File.ReadAllText(m_setting_data_path);
        Debug.Log(json_data);
        Setting = JsonUtility.FromJson<SettingData>(json_data);

        Screen.SetResolution((int)Setting.Resolution.x, (int)Setting.Resolution.y, Screen.fullScreen);
    }

    public void SaveSettingData()
    {
        Setting.Resolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

        var json_data = JsonUtility.ToJson(Setting);
        File.WriteAllText(m_setting_data_path, json_data);
    }
}
