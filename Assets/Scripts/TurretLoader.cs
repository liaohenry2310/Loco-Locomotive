using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLoader : MonoBehaviour
{
    public void Reloadammo()
    {
        TurretCannon turretCannon = gameObject.GetComponentInParent<TurretCannon>();
        turretCannon.Reload();
        Debug.Log("reload ammo");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretLoader = this;
            //Reloadammo();
            //RepairTurret();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretLoader = null;
        }
    }
}
