using UnityEngine;

[System.Serializable]
public abstract class QuestBase
{
    [Header("퀘스트의 일부에서의 ID")]
    [SerializeField] private int m_particular_id;
    public int ID
    {
        get { return m_particular_id; }
    }

    [Header("CLEAR 상태의 여부")]
    [SerializeField] private bool m_is_particular_clear;
    public bool IsParticularClear
    {
        get { return m_is_particular_clear; }
        set { m_is_particular_clear = value; }
    }

    public abstract string GetProgressText();
}