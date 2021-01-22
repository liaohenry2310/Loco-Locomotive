using Interfaces;
using Items;
using UnityEngine;

namespace Dispenser
{

    public class Dispenser : MonoBehaviour, IInteractable
    {
        [SerializeField] private DispenserItem _dispenserItem = null;
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        private Animator _animator = null;
        private readonly int _active = Animator.StringToHash("Active");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
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
            }
        }

    }
}