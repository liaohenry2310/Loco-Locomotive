using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IInteractable
{
    public DispenserData.Type Type { get; set; } = DispenserData.Type.None;

    public BoxCollider2D boxCollider2D = null;
    private SpriteRenderer _spriteRenderer = null;

    public Vector3 itemOffset;

    public ItemIndicator itemIndicator;
    public float count = 5.0f;
    private void Awake()
    {
        _ = TryGetComponent(out _spriteRenderer);
    }
    private void Update()
    {
        if (boxCollider2D.enabled == true)
        {
            count -= Time.deltaTime;
            if (count <= 0.0f)
            {
                Destroy(gameObject);
            }

        }
        else
        {
            count = 5.0f;
        }
    }
    public void Setup(ref DispenserItem dispenserItem)
    {
        _spriteRenderer.sprite = dispenserItem.ItemSprite;
        Type = dispenserItem.Type;
    }

    public void Pickup(ref PlayerV1 player)
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

    public void Interact(PlayerV1 player)
    {
        Pickup(ref player);
    }
}
