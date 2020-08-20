using System;
using UnityEngine;

public class FireBox : MonoBehaviour
{
    public event Action OnReloadFuel;

    public void AddFuel()
    {
        OnReloadFuel?.Invoke();
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
