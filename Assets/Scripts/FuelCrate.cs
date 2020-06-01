using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCrate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.fuelCrate = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.fuelCrate = null;
        }
    }
}
