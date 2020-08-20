using UnityEngine;

[CreateAssetMenu(fileName = "MissileDataObject", menuName = "Weapon/Ammo/Missile")]
public class MissileData : ScriptableObject
{
    [SerializeField] private float _damage = 0.0f;
    [SerializeField] private float _minSpeed = 0.0f;
    [SerializeField] private float _maxSpeed = 0.0f;
    [SerializeField] private float _acceleration = 0.0f;
    [SerializeField] private float _radiusEffect = 0.0f;
    [SerializeField] private DispenserData.Type _type = DispenserData.Type.Missile;

    public float Damage => _damage;
    public float MinSpeed => _minSpeed;
    public float MaxSpeed => _maxSpeed;
    public float Acceleration => _acceleration;
    public float RadiusEffect => _radiusEffect;
    public DispenserData.Type Type => _type;

}
