using Interfaces;
using UnityEngine;

/// <summary>
/// ---- LEGACY CODE -----
/// Should be deleted in the FUTURE.
/// Cyro.
/// </summary>
public class TurretRepair : MonoBehaviour
{
    private TurretHealth _turretHealth;

    private void Start()
    {
        if (!TryGetComponent(out _turretHealth))
        {
            Debug.LogWarning("Fail to load Turret Repair component!.");
        }
    }

    public void Interact(PlayerV1 player)
    {
        if (player.GetItem.ItemType == DispenserData.Type.RepairKit)
        {
            _turretHealth.RepairTurret();
            player.GetItem.DestroyAfterUse();
            Debug.Log("Repair complete!");
        }
    }

}
