using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100;
    public float currentHealth;
    private float restoreTime;
    private bool mIsTakingDamage;
    public HealthBar healthBar;
    public GameObject player;

    void Start()
    {
        currentHealth = playerHealth;
        healthBar.SetMaxHealth(playerHealth);
    }

    void Update()
    {
        if (currentHealth == playerHealth)
        {
            healthBar.gameObject.SetActive(false);
        }
        else if(currentHealth < playerHealth)
        {
            healthBar.gameObject.SetActive(true);
        }
    }
    public void Restore()
    {
        if (currentHealth < playerHealth)
        {
            if (restoreTime > 3.0f)
            {
                currentHealth += 30;
                restoreTime = 0;
            }
            restoreTime += Time.deltaTime;
        }
    }

    public bool IsAlive()
    {
        if (currentHealth <= 0.0f)
        {
            return false;
        }
        return true;
    }

    public void TakeDamage(float amount)
    {
        healthBar.gameObject.SetActive(true);
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, playerHealth);
        healthBar.SetHealth(currentHealth);
    }


    private void Died()
    {
        if (IsAlive() ==false)
        {
            Debug.Log("player died");
            player.SetActive(false);
            Invoke("Respawn", 5f);
        }

    }
    private void Respawn()
    {
        Debug.Log("Respawn");
        GameObject spawnpoint = GameObject.Find("PlayerSpwanPoint");
        player.transform.localPosition = spawnpoint.transform.localPosition;
        player.SetActive(true);
        currentHealth = playerHealth;
    }
}

