using System.Collections;
using UnityEngine;

public class RespawnPod : MonoBehaviour
{
    private Animator _animator = null;
    
    private void Awake()
    {
        if (!TryGetComponent(out _animator))
        {
            Debug.LogWarning("Failed to load Animator component.");
        }
    }

    public void AnimationRespawnPod()
    {
        StartCoroutine(PodAnimationCo());
    }


    public IEnumerator PodAnimationCo()
    {
        _animator.SetBool("Spawn", true);
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        yield return new WaitForSeconds(clips[1].length);
        _animator.SetBool("Spawn", false);
    }

}
