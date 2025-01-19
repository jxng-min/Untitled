using UnityEngine;

public class PlayerRun : MonoBehaviour, IState<PlayerCtrl>
{
    private PlayerCtrl m_player_ctrl;
    private float m_speed = 7.0f;

    public void ExecuteEnter(PlayerCtrl sender)
    {
        m_player_ctrl = sender;
        if(m_player_ctrl)
        {
            m_player_ctrl.Animator.SetBool("IsMove", true);
            m_player_ctrl.Animator.SetBool("LShift", true);
        }
    }

    public void ExecuteExit(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.SetBool("LShift", false);
        m_player_ctrl.Animator.SetBool("IsMove", false);
    }

    public void ExecuteUpdate(PlayerCtrl sender)
    {
        m_player_ctrl.Animator.SetFloat("MoveZ", m_player_ctrl.Direction.z);
        m_player_ctrl.Animator.SetFloat("MoveX", m_player_ctrl.Direction.x);

        Vector3 right_direction = new Vector3(m_player_ctrl.CameraArm.right.x, 0f, m_player_ctrl.CameraArm.right.z).normalized;
        Vector3 forward_direction = new Vector3(m_player_ctrl.CameraArm.forward.x, 0f, m_player_ctrl.CameraArm.forward.z).normalized;
        
        Vector3 final_direction = ((forward_direction * m_player_ctrl.Direction.z) + (right_direction * m_player_ctrl.Direction.x)).normalized;

        Vector3 velocity = final_direction * m_speed;

        m_player_ctrl.Model.forward = Vector3.Lerp(m_player_ctrl.Model.forward, forward_direction, Time.deltaTime * 15f);

        Vector3 final_position = m_player_ctrl.Rigidbody.position + velocity * Time.fixedDeltaTime; 
        m_player_ctrl.Rigidbody.MovePosition(final_position);
    }
}
