using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCreat : MonoBehaviour
{
    private void onTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.ammoCreat = this;
        }
    }
    private void onTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.ammoCreat = null;
        }
    }
}
