using Interfaces;
using UnityEngine;

namespace Items
{

    public class Item : MonoBehaviour, IInteractable
    {
        [SerializeField] private Vector3 _itemOffset = Vector3.zero;
        public DispenserData.Type ItemType { get; set; } = DispenserData.Type.None;

        public ItemIndicator itemIndicator;

        public bool dropitem = false;

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
        
         private void Update()
         {
        if (_collider.enabled == true)
        {
            _count -= Time.deltaTime;
            if (_count <= 0.0f)
            {
                Destroy(gameObject);
            }

        }
        else
        {
            _count = 5.0f;
        }
    }

        public void Setup(ref DispenserItem dispenserItem)
        {
            _spriteRenderer.sprite = dispenserItem.ItemSprite;
            ItemType = dispenserItem.Type;
        }

        public void Pickup(ref PlayerV1 player)
        {
            dropitem = false;
            transform.SetParent(player.transform);
            transform.position += _itemOffset;
            _collider.enabled = false;
        }

        public void DropItem()
        {
            dropitem = true;
            _collider.enabled = true;
            transform.SetParent(null);
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