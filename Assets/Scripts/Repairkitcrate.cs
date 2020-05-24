using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repairkitcrate : MonoBehaviour
{
    private void onTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.repairkitcrate = this;
        }
    }
    private void onTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.repairkitcrate = null;
        }
    }
}
