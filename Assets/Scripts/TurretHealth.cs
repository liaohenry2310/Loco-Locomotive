using System;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float mCurrentHealth;

    public bool IsDestroyed { get; private set; }

    private void Start()
    {
        mCurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        mCurrentHealth -= amount;
        mCurrentHealth = Mathf.Clamp(mCurrentHealth, 0.0f, MaxHealth);
        if (mCurrentHealth <= 0.0f)
        {
            IsDestroyed = true;
        }
    }

    public bool IsAlive()
    {
        if (mCurrentHealth <= 0.0f)
        {
            return false;
        }
        return true;
    }

    public void RepairTurret(float amount)
    {
        mCurrentHealth += amount;
        mCurrentHealth = Mathf.Clamp(mCurrentHealth, amount, MaxHealth);
        IsDestroyed = false;

    }

}