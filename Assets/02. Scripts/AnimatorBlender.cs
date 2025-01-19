using System.Collections;
using UnityEngine;

public class AnimatorBlender : MonoBehaviour
{
    public void BlendLerp(Animator animator, string Parameter_name, float to_anime_state, float duration)
    {
        if(duration == -1f)
        {
            animator.SetFloat(Parameter_name, to_anime_state);
            return;
        }

        StartCoroutine(SetState(animator, Parameter_name, to_anime_state, duration));
    }

    private IEnumerator SetState(Animator animator, string parameter_name, float to_anime_state, float duration)
    {
        float process = 0f;
        float current_state = animator.GetFloat(parameter_name);

        while(true)
        {
            animator.SetFloat(parameter_name, Mathf.Lerp(current_state, to_anime_state, process));

            process += Time.deltaTime / duration;

            if(process > 1.0f)
            {
                animator.SetFloat(parameter_name, to_anime_state);
                yield break;
            }

            yield return null;
        }
    }
}
