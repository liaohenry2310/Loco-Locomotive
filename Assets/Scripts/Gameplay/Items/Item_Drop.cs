using GamePlay;
using UnityEngine;
using Items;
using Interfaces;

public class Item_Drop : MonoBehaviour, IInteractable
{
    public DispenserItem[] ListItem;
    private DispenserItem _dispenserItem;
    private Vector3 _screenBounds;
    private float moveSpeed = 2.0f;
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
        _screenBounds = GameManager.GetScreenBounds;

        var itemSize = ListItem.Length;
        int randomItem = Random.Range(0, itemSize - 1);
        _dispenserItem = ListItem[randomItem];
    }


    private void FixedUpdate()
    {
        transform.position -= transform.up * (moveSpeed * Time.fixedDeltaTime);

        if ((transform.position.x >= _screenBounds.x) ||
            (transform.position.x <= -_screenBounds.x) ||
            (transform.position.y >= _screenBounds.y) ||
            (transform.position.y <= -_screenBounds.y))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Train>())
        {
            moveSpeed = 0;
            transform.position -= new Vector3(0.0f, 0.15f, 0.0f);
            if (_dispenserItem)
            {
                _spriteRenderer.sprite = _dispenserItem.ItemSprite;
            }
        }
    }

    public void Interact(PlayerV1 player)
    {
        if (!player.GetItem)
        {            
            Destroy(gameObject);
            GameObject itemGo = Instantiate(_dispenserItem.ItemPerfab, player.transform.position, Quaternion.identity);
            Item item = itemGo.GetComponent<Item>();
            item.Setup(ref _dispenserItem);
            item.Pickup(ref player);

        }
    }



}
