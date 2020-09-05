using UnityEngine;

public class ShieldTurret : MonoBehaviour
{
    public IReparable IReparable { get; set; }
    public IDamageable<float> IDamageble { get; set; }
    public SpriteRenderer SpriteShieldTurret { get; private set; }

    private void Awake()
    {
        SpriteShieldTurret = GetComponent<SpriteRenderer>();
    }

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
