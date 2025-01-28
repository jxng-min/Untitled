using UnityEngine;

public class AttackReset : StateMachineBehaviour
{
    [SerializeField] private string m_trigger_name;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(m_trigger_name);
    }
}
