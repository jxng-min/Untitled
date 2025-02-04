using UnityEngine;

public class AnimationEventSender : MonoBehaviour
{
    private PlayerAttackState m_player_attack;
    private PlayerSkill1State m_player_skill1;
    private PlayerSkill2State m_player_skill2;
    private PlayerSkill3State m_player_skill3;

    private void Start()
    {
        m_player_attack = GetComponentInParent<PlayerAttackState>();
        m_player_skill1 = GetComponentInParent<PlayerSkill1State>();
        m_player_skill2 = GetComponentInParent<PlayerSkill2State>();
        m_player_skill3 = GetComponentInParent<PlayerSkill3State>();
    }

    public void Combo_Enable()
    {
        m_player_attack?.Combo_Enable();
    }

    public void Combo_Disable()
    {
        m_player_attack?.Combo_Disable();
    }

    public void Combo_Exist()
    {
        m_player_attack?.Combo_Exist();
    }

    public void Skill1_End()
    {
        m_player_skill1?.Skill1_End();
    }

    public void Skill2_End()
    {
        m_player_skill2?.Skill2_End();
    }

    public void Skill3_End()
    {
        m_player_skill3?.Skill3_End();
    }
}
