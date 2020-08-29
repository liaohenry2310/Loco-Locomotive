using UnityEngine;

public class EMPTurret : MonoBehaviour
{
    public IReparable IReparable { get; set; }
    public IDamageable<float> IDamageable { get; set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            player.reparableActions = IReparable;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            player.reparableActions = null;
        }
    }
}
