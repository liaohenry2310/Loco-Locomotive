using UnityEngine;

public class FireBox : MonoBehaviour
{
    private FuelController _fuelController;

    void Start()
    {
        _fuelController = GetComponentInParent<FuelController>();
    }

    public void AddFuel()
    {
        _fuelController.Reload();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            // safety first.
            if (player)
            {
                player.fireBox = this;
            }
            else
            {
                // log error.
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.fireBox = null;
        }
    }
}
