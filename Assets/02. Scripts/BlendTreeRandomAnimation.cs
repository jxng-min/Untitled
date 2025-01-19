using UnityEngine;

public class BlendTreeRandomAnimation : StateMachineBehaviour
{
    [Header("Parameter name")]
    [SerializeField] private string m_state_parameter_name;

    [Header("Blending Time")]
    [SerializeField] private float m_blend_duration = 0.5f;

    [Space(50)]
    [Header("Clips' Executing Time")]
    [SerializeField] private float[] m_clip_lengths;

    private AnimatorBlender m_anime_blender;
    private bool m_is_already_executed;
    private float m_current_delay;
    private int m_current_clip_index;

    private void RefreshClip()
    {
        m_current_clip_index = Random.Range(0, m_clip_lengths.Length);
        m_current_delay = m_clip_lengths[m_current_clip_index];
    }

    private void PlayUpdatedClip(Animator animator)
    {
        RefreshClip();

        m_anime_blender.BlendLerp(animator, m_state_parameter_name, m_current_clip_index, m_blend_duration);
    }

    // 상태 머신이 실행되면 이 함수가 처음으로 호출된다.
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_is_already_executed)
        {
            return;
        }

        m_anime_blender = animator.GetComponent<AnimatorBlender>();

        RefreshClip();

        m_is_already_executed = true;
    }

    // 상태 머신이 플레이 중일 때 매 프레임 호출된다.
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_current_delay -= Time.deltaTime;

        if(m_current_delay < 0f)
        {
            PlayUpdatedClip(animator);
        }
    }
}
