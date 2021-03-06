using Interfaces;
using System.Collections;
using UnityEngine;

namespace Items
{

    public class Item : MonoBehaviour, IInteractable
    {
        public DispenserData.Type ItemType { get; set; } = DispenserData.Type.None;

        private SpriteRenderer _spriteRenderer = null;
        private BoxCollider2D _collider = null;
        private readonly float _count = 5.0f;
        private Coroutine _coroutine = null;

        private void Awake()
        {
            if (!TryGetComponent(out _spriteRenderer))
            {
                Debug.LogWarning("<color=red>Fail</color> to load SpriteRenderer component!.");
            }

            if (!TryGetComponent(out _collider))
            {
                Debug.LogWarning("<color=red>Fail</color> to load BoxCollider2D component!.");
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
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        public void DropItem()
        {
            _collider.enabled = true;
            transform.SetParent(null);
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(DropCountDown());
            }
        }

        private IEnumerator DropCountDown()
        {
            yield return new WaitForSeconds(_count);
            Destroy(gameObject);
        }

        public void Interact(PlayerV1 player)
        {
            if (!player.GetItem)
            {
                Pickup(ref player);
            }
        }

        public void DestroyAfterUse()
        {
            Destroy(gameObject);
        }
    }

}
