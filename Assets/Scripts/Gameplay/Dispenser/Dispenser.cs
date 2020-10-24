using Interfaces;
using UnityEngine;

public class Dispenser : MonoBehaviour, Interfaces.IInteractable
{
    [SerializeField] private DispenserItem _dispenserItem;

    private SpriteRenderer _spriteRenderer;

    public Vector3 itemOffset;

    private void Awake()
    {
        _ = TryGetComponent(out _spriteRenderer);
    }

    private void Start()
    {
        _spriteRenderer.sprite = _dispenserItem.DispenserSprite;
    }

    public void Interact(PlayerV1 player)
    { 
        Debug.Log($"{gameObject.name}");
        if (!player.GetChildCount)
        {
            GameObject itemGo = Instantiate(_dispenserItem.ItemPerfab, player.transform.position - itemOffset, Quaternion.identity);
            Item item = itemGo.GetComponent<Item>();
            item.Setup(ref _dispenserItem);
            item.Pickup(ref player);
        }
    }
}
