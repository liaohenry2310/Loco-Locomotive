using UnityEngine;

public class RespawnPod : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void AnimationRespawnPod(bool isAnimated)
    {
        animator.SetBool("Spawn", isAnimated);
    }

}
