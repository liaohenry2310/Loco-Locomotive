using Interfaces;
using System;
using UnityEngine;

public class Train : MonoBehaviour, IDamageable<float>
{
    public event Action<float> OnUpdateHealthUI;  // HealthUI Action
    public event Action<float> OnUpdateFuelUI;    // FuelUI Action
    public event Action<float> OnFuelReloadUI;    // FireBox Action
    public event Action OnGameOver;               // GameManager action

    #region Members

    [SerializeField] private TrainData _trainData = null;
    [SerializeField] private FireBox _fireBox = null;

    private Turret[] _listTurrets;
    // Health
    private float _currentHealth = 0f;
    // Fuel Controller
    private float _currentFuel = 0f;
    private bool _outOfFuel = false;

    #endregion

    public void Initialized()
    {
        _listTurrets = GetComponentsInChildren<Turret>();
        _currentHealth = _trainData.MaxHealth;
        _currentFuel = _trainData.MaxFuel;
    }

    private void Start()
    {
        Initialized();
    }

    private void OnEnable()
    {
        _fireBox.OnReloadFuel += ReloadFuel;
    }

    private void OnDisable()
    {
        _fireBox.OnReloadFuel -= ReloadFuel;
    }

    private void Update()
    {
        CurrentFuel(1.0f / _trainData.FuelRate);
    }

    private void ReloadFuel()
    {
        _currentFuel = _trainData.MaxFuel;
        float fuelPerc = _currentFuel / _trainData.MaxFuel;
        OnFuelReloadUI?.Invoke(fuelPerc);
    }

    public void CurrentFuel(float amount)
    {
        if (_outOfFuel) return;

        _currentFuel -= amount;
        _currentFuel = Mathf.Clamp(_currentFuel, 0.0f, _trainData.MaxFuel);
        float currentPercentage = _currentFuel / _trainData.MaxFuel;
        OnUpdateFuelUI?.Invoke(currentPercentage);
        if (_currentFuel < 0.01f)
        {
            Debug.Log("[FuelController] Game over.");
            OnGameOver?.Invoke();
            _outOfFuel = true;
            return;
        }
    }

    public Turret[] GetTurrets() => _listTurrets;

    public void TakeDamage(float damage)
    {
        if (_currentHealth >= 0.1f)
        {
            _currentHealth -= damage;
            float healthPerc = _currentHealth / _trainData.MaxHealth;
            OnUpdateHealthUI?.Invoke(healthPerc);
        }
    }
}
