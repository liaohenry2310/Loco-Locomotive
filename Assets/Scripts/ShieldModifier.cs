using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModifier : EnemyHealth
{
    public float ShieldHealth;


    public override void TakeDamage(float takingDamage) 
    {
        if (ShieldHealth <= 0.0f) // shiel is gone.
        {
            health -= takingDamage;
            if (health <= 0.0f)
            {
                Destroy(gameObject.transform.parent.gameObject);
                Debug.Log("See you next time!");
            }
        }
        else
        {
            ShieldHealth -= takingDamage;
            if (!IsShieldModifierOn())
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }


    public bool IsShieldModifierOn()
    {
        return ShieldHealth>0.0f;
    }

}
