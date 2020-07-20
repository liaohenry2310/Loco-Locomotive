using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private DispenserData _dispenserData = default;
    [SerializeField] private DispenserData.Type _dispenserDataType = default;

    private Color _ItemColor;

    private void Start()
    {
        SpriteRenderer go = GetComponentInChildren<SpriteRenderer>();
        go.color = _dispenserData.ColorByItemType[_dispenserDataType];
        _ItemColor = go.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (!player)
            {
                Debug.LogError($"Dispenser {gameObject.name} failed to find player");
                return;
            }
            
            player.PickUpItem(true, _dispenserDataType, _ItemColor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (!player)
            {
                Debug.LogError($"Dispenser {gameObject.name} failed to find player");
                return;
            }
            
            player.PickUpItem(false, DispenserData.Type.None, Color.white);
        }
    }
}
