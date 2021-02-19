using System;
using Turret;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainDataObject", menuName = "Train/Train")]
public class TrainData : ScriptableObject, ISerializationCallbackReceiver
{
    public float MaxHealth = 2500.0f;
    public float MaxFuel = 2500.0f;
    public float FuelRate = 10.0f;

    [NonSerialized] public int PlayerCount = 1;
    [NonSerialized] public float CurrentHealth;
    [NonSerialized] public float CurrentFuel;
    [NonSerialized] public TurretBase[] ListTurret;
    [NonSerialized] public Transform TrainTransform;

    public void OnAfterDeserialize()
    {
        CurrentHealth = MaxHealth;
        CurrentFuel = MaxFuel;
    }

    public void OnBeforeSerialize()
    { }

    public void Initialize(MonoBehaviour monoBehaviour)
    {
        CurrentHealth = MaxHealth;
        CurrentFuel = MaxFuel;
        TrainTransform = monoBehaviour.transform;
    }

    public float FuelPercentage => CurrentFuel / MaxFuel;
    public float HealthPercentage => CurrentHealth / MaxHealth;
}