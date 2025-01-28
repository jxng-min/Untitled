using UnityEngine;

public class AnimationEventSender : MonoBehaviour
{
    private PlayerAttackState Player;

    private void Start()
    {
        Player = GetComponentInParent<PlayerAttackState>();
    }

    public void Combo_Enable()
    {
        Player?.Combo_Enable();
    }

    public void Combo_Disable()
    {
        Player?.Combo_Disable();
    }

    public void Combo_Exist()
    {
        Player?.Combo_Exist();
    }
}
