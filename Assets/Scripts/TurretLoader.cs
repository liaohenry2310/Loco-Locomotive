using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLoader : MonoBehaviour
{
    public void Reloadammo(int ammoCount)
    {

    }
    private void onTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretLoader = this;
        }
    }
    private void onTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretLoader = null;
        }
    }
}
