using UnityEngine;

public class PlayerDeadState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            SetRandomTrigger();
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        // TODO: 다시하기 버튼 생기면 IDLE 전환 로직 추가
    }

    public void ExecuteExit(PlayerCtrl sender)
    {

    }

    private void SetRandomTrigger()
    {
        int value = Random.Range(0, 2);
        if(value == 0)
        {
            m_player_ctrl.Animator.SetTrigger("Dead1");
        }
        else
        {
            m_player_ctrl.Animator.SetTrigger("Dead2");
        }
    }
}