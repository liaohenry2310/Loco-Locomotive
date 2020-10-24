using Interfaces;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public DispenserData.Type Type { get; set; } = DispenserData.Type.None;

    public BoxCollider2D boxCollider2D = null;
    private SpriteRenderer _spriteRenderer = null;

    public Vector3 itemOffset;

    public ItemIndicator itemIndicator;
    public bool dropitem = false;

    private void Awake()
    {
        _ = TryGetComponent(out _spriteRenderer);
    }

    public void Setup(ref DispenserItem dispenserItem)
    {
        _spriteRenderer.sprite = dispenserItem.ItemSprite;
        Type = dispenserItem.Type;
    }

    public void Pickup(ref PlayerV1 player)
    {
        dropitem = false;
        transform.SetParent(player.transform);
        transform.position += itemOffset;
        boxCollider2D.enabled = false;
    }

    public void DropItem()
    {
        dropitem = true;
        boxCollider2D.enabled = true;
        transform.SetParent(null);
    }

    public void Interact(PlayerV1 player)
    {
        Pickup(ref player);
    }




}
