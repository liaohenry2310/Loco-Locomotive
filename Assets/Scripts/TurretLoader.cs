using System;
using UnityEngine;

public class TurretLoader : MonoBehaviour
{

    public event Action<DispenserData.Type> OnReloadTurret;

    public void Reloadammo(DispenserData.Type type)
    {
        // New code
        OnReloadTurret?.Invoke(type);

        // Old code
        //TurretCannon turretCannon = gameObject.GetComponentInParent<TurretCannon>();
        //turretCannon.Reload(type);
        //Debug.Log("reload ammo");
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
