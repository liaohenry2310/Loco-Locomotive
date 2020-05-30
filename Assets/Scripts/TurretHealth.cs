using System;
using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public float MaxHealth = 100f;

    private float mCurrentHealth;

    public bool IsDestroyed { get; private set; }

    private void Start()
    {
        mCurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        mCurrentHealth -= amount;
        if (mCurrentHealth <= 0.0f)
        {
            IsDestroyed = true;
        }
    }

}
