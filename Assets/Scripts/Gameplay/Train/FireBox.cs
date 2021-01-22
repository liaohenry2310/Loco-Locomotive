using Interfaces;
using Items;
using System;
using UnityEngine;

public class FireBox : MonoBehaviour, IInteractable
{
    public event Action OnReloadFuel;
    private Animator _animator = null;
    private readonly int _active = Animator.StringToHash("Active");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Interact(PlayerV1 player)
    {
        Item item = player.GetItem;
        if (item != null && item.ItemType == DispenserData.Type.Fuel)
        {
            _animator.SetTrigger(_active);
            OnReloadFuel?.Invoke();
            item.DestroyAfterUse();
        }
    }

}
