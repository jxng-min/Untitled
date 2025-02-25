using UnityEngine;

public class AnimationEventSender : MonoBehaviour
{
    private PlayerAttackState m_player_attack;
    private PlayerSkill1State m_player_skill1;
    private PlayerSkill2State m_player_skill2;
    private PlayerSkill3State m_player_skill3;
    private PlayerSkill4State m_player_skill4;
    private PlayerSkill5State m_player_skill5;
    private PlayerSkill6State m_player_skill6;

    private void Start()
    {
        m_player_attack = GetComponentInParent<PlayerAttackState>();
        m_player_skill1 = GetComponentInParent<PlayerSkill1State>();
        m_player_skill2 = GetComponentInParent<PlayerSkill2State>();
        m_player_skill3 = GetComponentInParent<PlayerSkill3State>();
        m_player_skill4 = GetComponentInParent<PlayerSkill4State>();
        m_player_skill5 = GetComponentInParent<PlayerSkill5State>();
        m_player_skill6 = GetComponentInParent<PlayerSkill6State>();
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

    public void Skill4_End()
    {
        m_player_skill4?.Skill4_End();
    }
    
    public void Skill5_End()
    {
        m_player_skill5?.Skill5_End();
    }

    public void Skill6_End()
    {

    }
}
