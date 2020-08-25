using UnityEngine;

[RequireComponent(typeof(FuelController))]
public class TrainController : MonoBehaviour
{
    
    [Header("Train Properties")]
    [SerializeField] private float FuelUpdateRate = 4.0f;

    private FuelController _fuelController;

    private void Awake()
    {
        _ = TryGetComponent(out _fuelController);
    }
    private void Update()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        _fuelController.CurrentFuel(player.Length / FuelUpdateRate);
    }
}
