using UnityEngine;

[CreateAssetMenu(fileName = "ShieldGeneratorDataObject", menuName = "Weapon/Defense/ShieldGenerator")]
public class ShieldGeneratorData : ScriptableObject
{
    [SerializeField] private float _chargeTime = 3.0f;
    [SerializeField] private float _coolDownTime = 15.0f;
    [SerializeField] private float _barrierDuration = 5.0f;
    [SerializeField] private float _maxHealth = 250.0f;
    [SerializeField] private Sprite[] sprites = null;

    public float ChargeTime => _chargeTime;
    public float CoolDownTime => _coolDownTime;
    public float BarrierDuration => _barrierDuration;
    public float MaxHealth => _maxHealth;

    public Sprite GetShieldSprites(int index)
    {
        return sprites[index % sprites.Length];
    }

}