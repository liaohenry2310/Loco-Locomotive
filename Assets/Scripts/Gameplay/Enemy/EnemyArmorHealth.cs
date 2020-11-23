using UnityEngine;

public class EnemyArmorHealth : MonoBehaviour
{
    // Armor type
    private float _armorHealth=0.0f;
    public float ArmorHealth {
        get {return _armorHealth; }
        set{ _armorHealth = value; }
    }
    public void TakeDamage(float takingDamage)
    {
        _armorHealth -= takingDamage;
        Debug.Log("[ArmorHealth] Lost " + takingDamage + "hp. Current health: " + _armorHealth);
        if (!ArmorIsActive())
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Armor SpriteRenderer !enabled");
        }
    }
    public bool ArmorIsActive()
    { return _armorHealth > 0.0; }

}
