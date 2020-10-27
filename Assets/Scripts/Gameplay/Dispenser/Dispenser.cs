using Interfaces;
using Items;
using UnityEngine;

namespace Dispenser
{

    public class Dispenser : MonoBehaviour, IInteractable
    {
        [SerializeField] private DispenserItem _dispenserItem;
        [SerializeField] private Vector3 itemOffset = Vector3.zero;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            if (!TryGetComponent(out _spriteRenderer))
            {
                Debug.LogWarning("Fail to load SpriteRenderer component!.");
            }

        }

        private void Start()
        {
            _spriteRenderer.sprite = _dispenserItem.DispenserSprite;
        }

        public void Interact(PlayerV1 player)
        {
            if (!player.GetItem)
            {
                GameObject itemGo = Instantiate(_dispenserItem.ItemPerfab, player.transform.position - itemOffset, Quaternion.identity);
                Item item = itemGo.GetComponent<Item>();
                item.Setup(ref _dispenserItem);
                item.Pickup(ref player);
            }
        }
    }
}