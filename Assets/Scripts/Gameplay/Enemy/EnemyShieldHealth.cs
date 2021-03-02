using UnityEngine;

public class EnemyShieldHealth : MonoBehaviour
{
    // Shield type
    bool reShield = false;
    public bool ReShield { get { return reShield; } set { reShield = value; } }
    private float _shieldHealth;
    private void Update()
    {
        if (reShield)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            reShield = false;
        }
    }
    public float ShieldHealth
    {
        get{return _shieldHealth; }
        set{ _shieldHealth=value; }
    }

    public void TakeDamage(float takingDamage)
    {
        _shieldHealth -= takingDamage;
        //Debug.Log("[ShieldHealth] Lost " + takingDamage + "hp. Current health: " + _shieldHealth);
        if (!ShieldIsActive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            //Debug.Log("Shield SpriteRenderer !enabled");
        }
    }
    public bool ShieldIsActive => _shieldHealth > 0f;

}
