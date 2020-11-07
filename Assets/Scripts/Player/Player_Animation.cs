using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Animation : MonoBehaviour
{
    private PlayerInput _playerInput;
    public RuntimeAnimatorController[] _runtimeAnimatorController;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!TryGetComponent(out _playerInput))
        {
            Debug.LogWarning("Fail to load Player Input component!.");
        }
    }

    private void Update()
    {
        for (int i = 0; i < 4; ++i) 
        {
            if (_playerInput.playerIndex == i)
            {
                animator.runtimeAnimatorController = _runtimeAnimatorController[i];
            }
        }
    }

}
