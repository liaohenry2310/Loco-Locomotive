using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLoader : MonoBehaviour
{
    public void Reloadammo()
    {
        TurretCannon turretCannon = gameObject.GetComponent<TurretCannon>();
        turretCannon.Repair();
        Debug.Log("reload ammo");
    }
    public void RepairTurret()
    {
        TurretCannon turretCannon = gameObject.GetComponent<TurretCannon>();
        turretCannon.Reload();
        Debug.Log("repair turret");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretLoader = this;
            Reloadammo();
            RepairTurret();
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
