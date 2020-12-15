using UnityEngine;

/// <summary>
/// ---- LEGACY CODE -----
/// Should be deleted in the FUTURE.
/// Cyro.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100;
    public float currentHealth;
    private float restoreTime;
    private bool mIsTakingDamage;
    private bool takeDamage = false;
    private bool restore = false;

    public HealthBar healthBar;
    public GameObject player;

    void Start()
    {
        currentHealth = playerHealth;
        healthBar.SetMaxHealth(playerHealth);
    }

    void Update()
    {
        if (currentHealth >= playerHealth)
        {
            currentHealth = playerHealth;
            healthBar.gameObject.SetActive(false);
        }
        else if(currentHealth < playerHealth && currentHealth>0.0f)
        {
            healthBar.gameObject.SetActive(true);
        }

        restoreTime += Time.deltaTime;
        if (restoreTime > 3.0f)
        {
            restore = true;
            takeDamage = false;
            restoreTime = 0;
        }
        if (currentHealth > 0.0f && currentHealth < playerHealth && restore == true && takeDamage == false)
        {
            currentHealth += 30;
            healthBar.SetHealth(currentHealth);
            restore = false;
        }
        if (IsAlive() == false)
        {
            Debug.Log("player died");
            player.SetActive(false);
            Invoke("Respawn", 5f);
        }
    }
    public bool IsAlive()
    {
        return currentHealth > 0f;
    }
    public void TakeDamage(float amount)
    {
        restoreTime = 0;
        takeDamage = true;
        healthBar.gameObject.SetActive(true);
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, playerHealth);
        healthBar.SetHealth(currentHealth);
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

