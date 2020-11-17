using Interfaces;
using System.Collections;
using UnityEngine;

namespace Items
{

    public class Item : MonoBehaviour, IInteractable
    {
        public DispenserData.Type ItemType { get; set; } = DispenserData.Type.None;
        public ItemIndicator itemIndicator;

        private SpriteRenderer _spriteRenderer = null;
        private BoxCollider2D _collider = null;
        private float _count = 5.0f;

        private void Awake()
        {
            if (!TryGetComponent(out _spriteRenderer))
            {
                Debug.LogWarning("Fail to load SpriteRenderer component!.");
            }

            if (!TryGetComponent(out _collider))
            {
                Debug.LogWarning("Fail to load BoxCollider2D component!.");
            }
        }

        public void Setup(ref DispenserItem dispenserItem)
        {
            _spriteRenderer.sprite = dispenserItem.ItemSprite;
            ItemType = dispenserItem.Type;
        }

        public void Pickup(ref PlayerV1 player)
        {
            transform.SetParent(player.transform);
            transform.position = player.PlayerItemPlaceHolder;
            _collider.enabled = false;
        }

        public void DropItem()
        {
            _collider.enabled = true;
            transform.SetParent(null);
            StartCoroutine(DropCountDown());
        }

        private IEnumerator DropCountDown()
        {
            yield return new WaitForSeconds(_count);
            Destroy(gameObject);
        }

        public void Interact(PlayerV1 player)
        {
            Pickup(ref player);
        }

        public void DestroyAfterUse()
        {
            Destroy(gameObject);
        }
    }

}