using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRepair : MonoBehaviour
{
    public void Repair()
    {
        TurretHealth turretHealth = gameObject.GetComponent<TurretHealth>();
        turretHealth.RepairTurret();
        Debug.Log("repair turret");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretRepair = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretRepair = null;
        }
    }
}
