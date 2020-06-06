using System;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float mCurrentHealth;

    public bool IsDestroyed { get; private set; }
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
        if (mCurrentHealth <= 0.0f)
        {
            IsDestroyed = true;
        }
        healthBar.SetHealth(mCurrentHealth);
    }

    public void RepairTurret()
    {
        mCurrentHealth += repairHealth;
        mCurrentHealth = Mathf.Clamp(mCurrentHealth, repairHealth, MaxHealth);
        IsDestroyed = false;
        healthBar.SetHealth(mCurrentHealth);

    }

}