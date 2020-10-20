using Interfaces;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private DestroyOnDelay _delayOnDestroy = null;

    public DispenserData.Type Type { get; set; } = DispenserData.Type.None;

    public BoxCollider2D boxCollider2D = null;
    public SpriteRenderer _spriteRenderer = null;

    public Vector3 itemOffset;

    public ItemIndicator itemIndicator;

    public void StartDestructionTimer()
    {
        if (_delayOnDestroy)
        {
            _delayOnDestroy.BeginTimer();
        }
    }

    public void Setup(ref DispenserItem dispenserItem)
    {
        _spriteRenderer.sprite = dispenserItem.ItemSprite;
        Type = dispenserItem.Type;
    }

    public void Pickup(ref Player player)
    {
        transform.SetParent(player.transform);
        transform.position += itemOffset;
        boxCollider2D.enabled = false;
    }

    public void DropItem()
    {
        boxCollider2D.enabled = true;
        transform.SetParent(null);
    }

    public void Interact(Player player)
    {
        Pickup(ref player);
    }

    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
