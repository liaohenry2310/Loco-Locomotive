using System;
using System.ComponentModel;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    public event Action<float> OnUpdateFuelUI;
    public event Action<float> OnFuelReloadUI;
    public event Action OnGameOver;

    [Header("Fuel Controller Properties")]
    [SerializeField] private float _maxFuel = 100f;
    [SerializeField] private float _ammountToReload = 50f;

    [ReadOnly(true)] private float currentFuel;
    private bool outOfFuel = false;

    void Start()
    {
        if (GameManager.Instance)
        {
            OnGameOver += GameManager.Instance.GameOver;
        }
        else
        {
            Debug.LogWarning($"[FuelController] -- GameManager.Instance is null.");
        }
        currentFuel = _maxFuel;
    }

    public void Reload()
    {
        currentFuel += _ammountToReload;
        OnFuelReloadUI?.Invoke(_ammountToReload);
    }

    public void CurrentFuel(float amount)
    {
        if (outOfFuel) return;

        currentFuel -= amount;
        currentFuel = Mathf.Clamp(currentFuel, 0.0f, _maxFuel);
        float currentPercentage = currentFuel / _maxFuel;
        OnUpdateFuelUI?.Invoke(currentPercentage);
        if (currentFuel < 0.01f)
        {
            Debug.Log("[FuelController] Game over.");
            OnGameOver?.Invoke();
            outOfFuel = true;
            return;
        }
    }

}
