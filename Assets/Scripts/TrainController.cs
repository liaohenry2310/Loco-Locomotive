using UnityEngine;

[RequireComponent(typeof(FuelController))]
public class TrainController : MonoBehaviour
{

    [Header("Train Properties")]
    [SerializeField] private float FuelUpdateRate = 4.0f;

    private FuelController _fuelController;

    private void Start()
    {
        _ = TryGetComponent(out _fuelController);
    }

    private void Update()
    {
        _fuelController.CurrentFuel(1.0f / FuelUpdateRate);
    }
}
