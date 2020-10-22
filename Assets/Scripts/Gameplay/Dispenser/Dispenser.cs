using UnityEngine;

public class Dispenser : MonoBehaviour, Interfaces.IInteractable
{
    [Header("Attributes")]
    [SerializeField] private DispenserItem _dispenserItem = default;

    private void Start()
    {
        SpriteRenderer go = GetComponentInChildren<SpriteRenderer>();
        go.sprite = _dispenserItem.sprite;
        go.color = _dispenserItem.DispenserColor;
    }

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

    public void Interact(PlayerV1 player)
    {
        Debug.Log($"{gameObject.name}");
    }
}
