using System;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    public event Action<float> OnUpdateFuelUI;
    public event Action<float> OnFuelReloadUI;

    public event Action OnGameOver;

    [Header("Properties")]
    [SerializeField] private float MaxFuel = 100f;

    private float currentFuel;

    void Start()
    {
        currentFuel = MaxFuel;
    }

    public void Reload(float amount)
    {
        OnFuelReloadUI?.Invoke(amount);
    }

    public void CurrentFuel(float amount)
    {
        currentFuel -= amount;
        float currentPercentage = currentFuel / MaxFuel;
        OnUpdateFuelUI?.Invoke(currentPercentage);
        if (currentFuel <= 0.01f)
        {
            Debug.Log("[FuelController] Game over son!");
            OnGameOver?.Invoke();
        }
    }

}
