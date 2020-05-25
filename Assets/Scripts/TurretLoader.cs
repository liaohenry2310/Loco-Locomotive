using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLoader : MonoBehaviour
{
    public TurretCannon turretCannon;
    public int currentammo = 30;
    public float repairhealth = 30;
    public void Reloadammo()
    {
        turretCannon.ammo = currentammo;
    }
    public void RepairTurret()
    {
        turretCannon.repairHealth = repairhealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretLoader = this;
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
