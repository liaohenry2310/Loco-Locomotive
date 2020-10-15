using Interfaces;
using UnityEngine;

public class EMPGenerator : IReparable, IDamageable<float>
{
    private readonly HealthBar _healthBar;
    private readonly float _maxHealth;
    private float _curretHealth = 0.0f;

    public float ChargerTimer { get; set; } = 0f;
    public bool CoolDownToActivated { get; set; } = false;

    public bool IsReadyToUse { get; set; } = true;

    public EMPGenerator(HealthBar healthBar, float maxHealth)
    {
        _healthBar = healthBar;
        _maxHealth = maxHealth;
        _curretHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    public bool IsAlive()
    {
        return _curretHealth > 0f;
    }

    public void Repair()
    {
        _curretHealth = _maxHealth;
        _healthBar.SetHealth(_curretHealth);
        Debug.Log("[EMPGenerator] -- Repair complete.");
    }

    public void TakeDamage(float amount)
    {
        _curretHealth -= amount;
        _curretHealth = Mathf.Clamp(_curretHealth, 0f, _maxHealth);
        _healthBar.SetHealth(_curretHealth);
    }
   
}
