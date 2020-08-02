using System;
using UnityEngine;

public class FuelController :MonoBehaviour
{
    public event Action<float> OnUpdateFuelUI;
    public event Action<float> OnFuelReloadUI;
    public event Action OnGameOver;

    [Header("Properties")]
    [SerializeField] private float MaxFuel = 100f;

    public float currentFuel;

    void Start()
    {
        OnGameOver += GameManager.Instance.GameOver;
        currentFuel = MaxFuel;
    }

    public void Reload(float amount)
    {
        currentFuel += amount;
        OnFuelReloadUI?.Invoke(amount);
    }

    public void CurrentFuel(float amount)
    {
        currentFuel -= amount;
        currentFuel = Mathf.Clamp(currentFuel, 0.0f, MaxFuel);
        if (currentFuel <= 0.01f)
        {
            Debug.Log("[FuelController] Game over son!");
            OnGameOver?.Invoke();
            return;
        }
        float currentPercentage = currentFuel / MaxFuel;
        OnUpdateFuelUI?.Invoke(currentPercentage);
    }

}
