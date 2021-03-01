using Interfaces;
using Items;
using System;
using UnityEngine;

public class FireBox : MonoBehaviour, IInteractable
{
    public event Action OnReloadFuel;
    public AudioClip clip;
    private AudioSource _audioSource;
    private Animator _animator = null;
    private readonly int _active = Animator.StringToHash("Active");

    private void Awake()
    {
        if (!TryGetComponent(out _audioSource))
        {
            Debug.LogWarning("Fail to load Audio Source component.");
        }

       _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _audioSource.volume = 0.5f;
    }

    public void Interact(PlayerV1 player)
    {
        Item item = player.GetItem;
        if (item != null && item.ItemType == DispenserData.Type.Fuel)
        {
            _audioSource.PlayOneShot(clip);
            _animator.SetTrigger(_active);
            OnReloadFuel?.Invoke();
            item.DestroyAfterUse();
        }
    }

}
