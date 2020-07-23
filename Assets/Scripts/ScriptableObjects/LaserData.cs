using UnityEngine;

[CreateAssetMenu(fileName = "LaserDataObject", menuName = "Weapon/Laser")]
public class LaserData : ScriptableObject
{
    [SerializeField] private float _range = 0f;
    [SerializeField] private float _damage = 0f;
    [SerializeField] private float _ammo = 0f;
    [SerializeField] private DispenserData.Type _laserType = DispenserData.Type.LaserBeam;

    public float Range => _range;
    public float Damage => _damage;
    public float Ammo { get => _ammo; set { _ammo = value; } }
    public DispenserData.Type LaserType => _laserType;
}
