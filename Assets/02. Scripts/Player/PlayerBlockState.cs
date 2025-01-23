using UnityEngine;

public class PlayerBlockState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetBool("IsBlock", true);
            m_player_ctrl.IsBlock = true;

            if(m_player_ctrl.Direction.magnitude > 0f)
            {
                m_player_ctrl.Animator.SetBool("IsMove", true);
            }
            
        }
    }

    public void Execute(PlayerCtrl sender)
    {
       // m_player_ctrl.Move(5f);
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.SetBool("IsBlock", false);
        m_player_ctrl.IsBlock = false;

        m_player_ctrl.Animator.SetBool("IsMove", false);
    }
}
