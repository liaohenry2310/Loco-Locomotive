using UnityEngine;

public class EnemyShieldHealth : MonoBehaviour
{
    // Shield type
    private float _shieldHealth;
    public float ShieldHealth
    {
        get{return _shieldHealth; }
        set{ _shieldHealth=value; }
    }

    public void TakeDamage(float takingDamage)
    {
        _shieldHealth -= takingDamage;
        Debug.Log("[ShieldHealth] Lost " + takingDamage + "hp. Current health: " + _shieldHealth);
        if (!ShieldIsActive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Shield SpriteRenderer !enabled");
        }
    }
    public bool ShieldIsActive => _shieldHealth > 0f;

}
