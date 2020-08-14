using UnityEngine;

public class EnemyShieldHealth : MonoBehaviour
{
    // Shield type
    public float ShieldHealth;

    public void TakeDamage(float takingDamage)
    {
        ShieldHealth -= takingDamage;
        Debug.Log("[ShieldHealth] Lost " + takingDamage + "hp. Current health: " + ShieldHealth);
        if (!ShieldIsActive())
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Shield SpriteRenderer !enabled");
        }
    }
    public bool ShieldIsActive()
    { return ShieldHealth > 0.0; }

}
