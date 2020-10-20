using Interfaces;
using UnityEngine;

public class Dispenser : MonoBehaviour,IInteractable
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

    public void Interact(Player player)
    {
        //if(!player.GetChildCount)
        //{
        GameObject itemGo = Instantiate(_dispenserItem.ItemPerfab, player.transform.position - itemOffset, Quaternion.identity);
        Item item = itemGo.GetComponent<Item>();   
        //}
    }


    //set up the sprite for the dispenser


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Player player = collision.GetComponent<Player>();
    //        if (!player)
    //        {
    //            Debug.LogError($"Dispenser {gameObject.name} failed to find player");
    //            return;
    //        }

    //        player.SetCurrentDispenser(_dispenserItem);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Player player = collision.GetComponent<Player>();
    //        if (!player)
    //        {
    //            Debug.LogError($"Dispenser {gameObject.name} failed to find player");
    //            return;
    //        }

    //        player.SetCurrentDispenser(null);
    //    }
    //}


}
