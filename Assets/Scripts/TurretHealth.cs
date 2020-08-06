using System;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float mCurrentHealth;

    public HealthBar healthBar;
    public float repairHealth = 20;
    private void Start()
    {
        mCurrentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    public void TakeDamage(float amount)
    {
        mCurrentHealth -= amount;
        mCurrentHealth = Mathf.Clamp(mCurrentHealth, 0.0f, MaxHealth);
        healthBar.SetHealth(mCurrentHealth);
    }

    public bool IsAlive => mCurrentHealth > 0.0f;

    public void RepairTurret()
    {
        mCurrentHealth += repairHealth;
        mCurrentHealth = Mathf.Clamp(mCurrentHealth, repairHealth, MaxHealth);
        healthBar.SetHealth(mCurrentHealth);
    }
}




