using System;
using UnityEngine;

public class TurretLoader : MonoBehaviour
{

    public event Action<DispenserData.Type> OnReloadTurret;

    public void Reloadammo(DispenserData.Type type)
    {
        OnReloadTurret?.Invoke(type);
    }

    //TODO: Refactor
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Player player = collision.GetComponent<Player>();
    //        player.turretLoader = this;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Player player = collision.GetComponent<Player>();
    //        player.turretLoader = null;
    //    }
    //}
}
