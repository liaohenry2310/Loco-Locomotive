using Interfaces;
using Items;
using System;
using System.Collections;
using UnityEngine;

public class FireBox : MonoBehaviour, IInteractable
{
    public event Action OnReloadFuel;
    private Animator _animator = null;

    private void Awake()
    {
        if (!TryGetComponent(out _animator))
        {
            Debug.LogWarning("Failed to load Animator component.");
        }
    }

    public void Interact(PlayerV1 player)
    {
        Item item = player.GetItem;
        if (item != null && item.ItemType == DispenserData.Type.Fuel)
        {
            StartCoroutine(FireBoxAnimationCo());
            OnReloadFuel?.Invoke();
            item.DestroyAfterUse();
        }
    }
    private IEnumerator FireBoxAnimationCo()
    {
        _animator.SetBool("AddFuel", true);
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        yield return new WaitForSeconds(clips[1].length);
        _animator.SetBool("AddFuel", false);
    }
}
