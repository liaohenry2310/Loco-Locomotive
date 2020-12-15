using UnityEngine;

/// <summary>
/// ---- LEGACY CODE -----
/// Should be deleted in the FUTURE.
/// Cyro.
/// </summary>
public class TurretHealth : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar = default;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;
    //public float repairHealth = 20;
    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0.0f, _maxHealth);
        _healthBar.SetHealth(_currentHealth);
    }

    public bool IsAlive => _currentHealth > 0.0f;

    public void RepairTurret()
    {
        //mCurrentHealth += repairHealth;
        //mCurrentHealth = Mathf.Clamp(mCurrentHealth, repairHealth, MaxHealth);
        _currentHealth = _maxHealth;
        _healthBar.SetHealth(_currentHealth);
    }
}




