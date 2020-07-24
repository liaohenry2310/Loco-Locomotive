using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable<float>
{
    // regular type 
    public float health;
    // Shield type
    public float ShieldHealth;

    public void TakeDamage(float takingDamage, DispenserData.Type damageType)
    {
        if (CompareTag("Enemy"))
        {
            health -= takingDamage;
            if (health <= 0.0f)
            {
                Destroy(gameObject);
                Debug.Log("See you next time!");
            }
        }

        if (CompareTag("ShieldEnemy"))
        {
            if (ShieldHealth <= 0.0f) // shiel is gone.
            {
                health -= takingDamage;
                if (health <= 0.0f)
                {
                    Destroy(gameObject);
                    Debug.Log("See you next time!");
                }
            }
            else
            {
                if (damageType != DispenserData.Type.LaserBeam)
                {
                    ShieldHealth -= takingDamage;
                    Debug.Log("Shield taking damage!");
                    if (!IsShieldModifierOn())
                    {
                        foreach (var go in GetComponentsInChildren<SpriteRenderer>())
                        {
                            if (go.CompareTag("ShieldEnemy"))
                            {
                                go.enabled = false;
                                break;
                            }
                        }
                        Debug.Log("Shield SpriteRenderer !enabled");
                    }
                }
            }
        }

    }

    public bool IsAlive()
    {
        return health > 0.0f;
    }

    //Shield type
    public bool IsShieldModifierOn()
    {
        return ShieldHealth > 0.0f;
    }
}
