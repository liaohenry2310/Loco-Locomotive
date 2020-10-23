using UnityEngine;

public class EnemyArmorHealth : MonoBehaviour
{
    // Armor type
    public float ArmorHealth;

    public void TakeDamage(float takingDamage)
    {
        ArmorHealth -= takingDamage;
        Debug.Log("[ArmorHealth] Lost " + takingDamage + "hp. Current health: " + ArmorHealth);
        if (!ArmorIsActive())
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Armor SpriteRenderer !enabled");
        }
    }
    public bool ArmorIsActive()
    { return ArmorHealth > 0.0; }

}
