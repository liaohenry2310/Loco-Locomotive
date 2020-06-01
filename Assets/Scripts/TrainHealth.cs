using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHealth : MonoBehaviour
{
    public float trainHealth;

    public float currentHealth;
    private float restoreTime;
    private bool mIsTakingDamage;

    public GameManager gameManager;

    void Start()
    {
        currentHealth = trainHealth;
    }

    private void Update()
    {

    }


    public void TakeDamage(float amount)
    {
        if (IsAlive())
        {
            currentHealth -= amount;
           // currentHealth = Mathf.Max(currentHealth, 0f);
            Debug.Log("[Health] Lost " + amount + "hp. Current health: " + currentHealth);
        }

        if (IsAlive() == false) 
        {
            gameManager.GameOver();
        }
    }

    public void Restore()
    {
       
        if(currentHealth <trainHealth)
        {
            if(restoreTime > 20f )
            {
                currentHealth += 5;
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
