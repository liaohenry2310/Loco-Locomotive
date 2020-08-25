using UnityEngine;

public class ShieldGenerator : IShieldGenerator
{
    private readonly ShieldGeneratorData _shieldGeneratorData;
    private readonly HealthBar _healthBar;
    private float _curretHealth = 0.0f;

    public float ChargerTimer { get; set; } = 0f;
    public bool CoolDownToActivated { get; set; } = false;

    public ShieldGenerator(ShieldGeneratorData data, HealthBar healthBar)
    {
        _healthBar = healthBar;
        _shieldGeneratorData = data;
        _curretHealth = data.MaxHealth;
        _healthBar.SetMaxHealth(data.MaxHealth);
    }

    public bool IsAlive()
    {
        return _curretHealth > 0f;
    }

    public void Repair()
    {
        _curretHealth = _shieldGeneratorData.MaxHealth;
        _healthBar.SetHealth(_curretHealth);
        Debug.Log($"[ShieldGenerator] -- Repair complete.");
    }

    public void TakeDamage(float amount)
    {
        _curretHealth -= amount;
        _curretHealth = Mathf.Clamp(_curretHealth, 0f, _shieldGeneratorData.MaxHealth);
        _healthBar.SetHealth(_curretHealth);
    }
}