using System;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    public event Action<float> OnUpdateFuelUI;
    public event Action<float> OnFuelReloadUI;
    public event Action OnGameOver;

    [Header("Fuel Controller Properties")]
    [SerializeField]
    private float _maxFuel = 100f;

    [SerializeField]
    private float _ammountToReload = 50f;
    private bool outOfFuel = false;

    public float currentFuel;

    void Start()
    {
        OnGameOver += GameManager.Instance.GameOver;
        currentFuel = _maxFuel;
    }

    public void Reload()
    {
        currentFuel += _ammountToReload;
        OnFuelReloadUI?.Invoke(_ammountToReload);
    }

    public void CurrentFuel(float amount)
    {
        if (outOfFuel)
            return;

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
