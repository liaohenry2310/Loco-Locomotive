using Interfaces;
using Items;
using UnityEngine;

namespace Dispenser
{

    public class Dispenser : MonoBehaviour, IInteractable
    {
        [SerializeField] private DispenserItem _dispenserItem;
        private bool _open =false;
        private SpriteRenderer _spriteRenderer;

        public Animator animator;
        private void Awake()
        {           
            animator = GetComponent<Animator>();
            if (!TryGetComponent(out _spriteRenderer))
            {
                Debug.LogWarning("Fail to load SpriteRenderer component!.");
            }
        }

        private void Start()
        {
            _spriteRenderer.sprite = _dispenserItem.DispenserSprite;
        }
        private void Update()
        {
            if (_open)
            {
                Invoke("unplayAnimation", 0.5f);
            }               
        }
        public void Interact(PlayerV1 player)
        {
            if (!player.GetItem)
            {
                _open = true;
                animator.SetBool("Open", true);
                GameObject itemGo = Instantiate(_dispenserItem.ItemPerfab, player.transform.position, Quaternion.identity);
                Item item = itemGo.GetComponent<Item>();
                item.Setup(ref _dispenserItem);
                item.Pickup(ref player);
            }
        }
        private void unplayAnimation()
        {
            _open = false;
            animator.SetBool("Open", false);
        }
    }
}