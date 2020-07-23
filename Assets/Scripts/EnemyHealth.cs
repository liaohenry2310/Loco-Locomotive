using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;


    void Update()
    {
        
    }
    public virtual void TakeDamage(float takingDamage) 
    {
        health -= takingDamage;
        if (health <= 0.0f)
        {
            Destroy(gameObject);
            Debug.Log("See you next time!");
        }
    }
    public bool IsAlive ()
    {
        return health > 0.0f;
    }
}
