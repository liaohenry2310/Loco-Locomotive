using Interfaces;
using Items;
using UnityEngine;

namespace Dispenser
{

    public class Dispenser : MonoBehaviour, IInteractable
    {
        [SerializeField] private DispenserItem _dispenserItem = null;
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
            if (!player.GetItem)
            {
                _animator.SetTrigger(_active);
                GameObject itemGo = Instantiate(_dispenserItem.ItemPerfab, player.transform.position, Quaternion.identity);
                Item item = itemGo.GetComponent<Item>();
                item.Setup(ref _dispenserItem);
                item.Pickup(ref player);
                _audioSource.PlayOneShot(clip);

            }
        }

    }
}