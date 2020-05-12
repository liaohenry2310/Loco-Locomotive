using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHealth : MonoBehaviour
{
    public float trainHealth;

    private float currentHealth;
    private float restoreTime;


    void Start()
    {
        currentHealth = trainHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsAlive())
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);
            Debug.Log("[Health] Lost " + amount + "hp. Current health: " + currentHealth);
        }
    }

    public void Restore()
    {
       
        if(currentHealth <trainHealth)
        {
            if(restoreTime > 20f)
            {
                currentHealth += 10;
                restoreTime = 0;
            }
            restoreTime += Time.deltaTime;
                    
        }
    }
    public bool IsAlive()
    {
        return currentHealth > 0f;
    }
}
