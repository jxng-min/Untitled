using UnityEngine;

public class PlayerRunState : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetBool("IsMove", true);
            m_player_ctrl.Animator.SetBool("LShift", true);
        }
    }

    public void Execute(PlayerCtrl sender)
    {
        Vector3 forward_direction = new Vector3(m_player_ctrl.CameraArm.forward.x, 0f, m_player_ctrl.CameraArm.forward.z);
        Vector3 right_direction = new Vector3(m_player_ctrl.CameraArm.right.x, 0f, m_player_ctrl.CameraArm.right.z);

        Vector3 final_direction = ((forward_direction * m_player_ctrl.Direction.z) + (right_direction * m_player_ctrl.Direction.x)).normalized;

        Vector3 velocity = final_direction * 7f;

        m_player_ctrl.Animator.SetFloat("MoveZ", m_player_ctrl.Direction.z);
        m_player_ctrl.Animator.SetFloat("MoveX", m_player_ctrl.Direction.x);

        m_player_ctrl.Model.forward = Vector3.Lerp(forward_direction, m_player_ctrl.Model.forward, Time.deltaTime * 15f);

        Vector3 new_position = m_player_ctrl.Rigidbody.position + velocity * Time.deltaTime;
        m_player_ctrl.Rigidbody.MovePosition(new_position);
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.SetBool("IsMove", false);
        m_player_ctrl.Animator.SetBool("LShift", false);
    }
}
