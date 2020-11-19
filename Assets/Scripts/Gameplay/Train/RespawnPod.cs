using UnityEngine;

public class RespawnPod : MonoBehaviour
{
    public Animator animator;
    [SerializeField]private PlayerV1 player =null;

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerV1>();
    }
    private void Update()
    {
        if (player._isRespawn) 
        {
            animator.SetBool("Respawn", true);
        }
        else
        {
            animator.SetBool("Respawn", false);
        }
    }

}
