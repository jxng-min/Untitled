using UnityEngine;

[System.Serializable]
public class SettingData
{
    [SerializeField] private bool m_background_sound_active;
    public bool BackgroundActive
    {
        get { return m_background_sound_active; }
        set { m_background_sound_active = value; }
    }

    [SerializeField] private float m_background_slider_value;
    public float Backgroundvalue
    {
        get { return m_background_slider_value; }
        set { m_background_slider_value = value;}
    }

    [SerializeField] private bool m_effect_sound_active;
    public bool EffectActive
    {
        get { return m_effect_sound_active; }
        set { m_effect_sound_active = value; }
    }

    [SerializeField] private float m_effect_slider_value;
    public float EffectValue
    {
        get { return m_effect_slider_value; }
        set { m_background_slider_value = value; }
    }

    [SerializeField] private bool m_camera_shaker_active;
    public bool CameraShakerActive
    {
        get { return m_camera_shaker_active; }
        set { m_camera_shaker_active = value; }
    }

    [SerializeField] private bool m_global_volume_active;
    public bool VolumeActive
    {
        get { return m_global_volume_active; }
        set { m_global_volume_active = value;}
    }

    [SerializeField] private Vector2 m_resolution_value;
    public Vector2 Resolution
    {
        get { return m_resolution_value; }
        set { m_resolution_value = value; }
    }
}
