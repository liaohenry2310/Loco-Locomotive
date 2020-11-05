using Turret;
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

    public uint PlayerCount { get; set; } = 1;
    private bool _outOfFuel = false;

    #endregion

    public void Initialized()
    {
        _trainData.TurretList = GetComponentsInChildren<TurretBase>();
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
        // Check how many player has on the scene to increate the FuelRate
        CurrentFuel(PlayerCount / _trainData.FuelRate);
    }

    private void ReloadFuel()
    {
        _trainData.CurrentFuel = _trainData.MaxFuel;
        float fuelPerc = _trainData.CurrentFuel / _trainData.MaxFuel;
        OnFuelReloadUI?.Invoke(fuelPerc);
    }

    public void CurrentFuel(float amount)
    {
        if (_outOfFuel) return;

        _trainData.CurrentFuel -= amount;
        _trainData.CurrentFuel = Mathf.Clamp(_trainData.CurrentFuel, 0.0f, _trainData.MaxFuel);
        float currentPercentage = _trainData.CurrentFuel / _trainData.MaxFuel;
        OnUpdateFuelUI?.Invoke(currentPercentage);
        if (_trainData.CurrentFuel < 0.01f)
        {
            Debug.Log("[FuelController] Game over.");
            OnGameOver?.Invoke();
            _outOfFuel = true;
            return;
        }
    }

    public void TakeDamage(float damage)
    {
        if (_trainData.CurrentHealth >= 0.1f)
        {
            _trainData.CurrentHealth -= damage;
            float healthPerc = _trainData.CurrentHealth / _trainData.MaxHealth;
            OnUpdateHealthUI?.Invoke(healthPerc);
        }
    }
}
