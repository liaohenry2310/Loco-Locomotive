using UnityEngine;

public class TurretRepair : MonoBehaviour
{
    private TurretHealth turretHealth;

    private void Start()
    {
        _ = TryGetComponent(out turretHealth);
    }

    public void Repair()
    {
        turretHealth.RepairTurret();
        Debug.Log("[TurretRepair] Repair complete!");
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
