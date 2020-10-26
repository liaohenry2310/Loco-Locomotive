using System;
using UnityEngine;

namespace Manager
{
    public class HealthSystem
    {

        public event Action OnUpdateHealthUI;

        public float Health { get; private set; } = 0.0f;
        private readonly float MaxHealth = 0.0f;

        public HealthSystem(float maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public float HealthPercentage => Health / MaxHealth;

        public void Damage(float damage)
        {
            Health -= damage;
            Health = Mathf.Clamp(Health, 0.0f, MaxHealth);
            OnUpdateHealthUI?.Invoke();
        }

        public void RestoreHealth(float restoreAmount)
        {
            Health += restoreAmount;
            Health = Mathf.Clamp(Health, 0.0f, MaxHealth);
            OnUpdateHealthUI?.Invoke();
        }

    }

}


