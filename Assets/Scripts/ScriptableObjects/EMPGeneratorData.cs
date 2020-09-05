using UnityEngine;

[CreateAssetMenu(fileName = "EMPGeneratorDataObject", menuName = "Weapon/Defense/EMPGenerator")]
public class EMPGeneratorData : ScriptableObject
{
    [SerializeField] private float _chargeTime = 3.0f;
    [SerializeField] private float _coolDownTime = 15.0f;
    [SerializeField] private float _shockWaveSpeedRate = 10.0f;
    [SerializeField] private float _maxHealth = 250.0f;
    [SerializeField] private Sprite[] sprites = null;

    public float ChargeTime => _chargeTime;
    public float CoolDownTime => _coolDownTime;
    public float ShockWaveSpeedRate => _shockWaveSpeedRate;
    public float MaxHealth => _maxHealth;

    public Sprite GetEMPSprites(int index)
    {
        return sprites[index % sprites.Length];
    }
}