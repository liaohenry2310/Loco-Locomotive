using System;
using Turret;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainDataObject", menuName = "Train/Train")]
public class TrainData : ScriptableObject, ISerializationCallbackReceiver
{
    public float MaxHealth = 2500.0f;
    public float MaxFuel = 2500.0f;
    public float FuelRate = 10.0f;

    [NonSerialized] public float CurrentHealth;
    [NonSerialized] public float CurrentFuel;
    [NonSerialized] public TurretBase[] TurretList;

    public void OnAfterDeserialize()
    {
        CurrentHealth = MaxHealth;
        CurrentFuel = MaxFuel;
    }

    public void OnBeforeSerialize()
    { }
}