using UnityEngine;

[CreateAssetMenu(fileName = "TrainDataObject", menuName = "Train/Train")]
public class TrainData : ScriptableObject
{
    [SerializeField] private float _maxHealth = 1000f;
    [SerializeField] private float _maxFuel = 2500f;
    [SerializeField] private float _fuelRate = 4f;

    public float MaxHealth => _maxHealth;
    public float MaxFuel => _maxFuel;
    public float FuelRate => _fuelRate;

}