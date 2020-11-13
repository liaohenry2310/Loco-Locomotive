using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turrets/Turret")]
public class TurretData : ScriptableObject
{
    [SerializeField] private float _maxHealth = 100.0f;
    [Range(50.0f, 360.0f)]
    [SerializeField] private float _aimSpeed = 55.0f;

    [Serializable]
    public struct MachineGun
    {
        public float damage;
        public float moveSpeed;
        public float fireRate;
        public float maxAmmo;
        public float spreadBullet;
        public AudioClip machinegunFire;
        public AudioClip machinegunBeam;
        public Sprite[] Uppersprites;
        public Sprite[] Cannonsprites;
        public Sprite[] Bottomsprites;

    }

    [Serializable]
    public struct MissileGun
    {
        public float damage;
        public float minSpeed;
        public float maxSpeed;
        public float acceleration;
        public float fireRate;
        public float radiusEffect;
        public float maxAmmo;
        public AudioClip missilegunFire;
        public AudioClip missilegunBeam;
        public Sprite[] Uppersprites;
        public Sprite[] Cannonsprites;
        public Sprite[] Bottomsprites;
        public LayerMask enemyLayerMask;
    }

    [Serializable]
    public struct LaserGun
    {
        public float damage;
        public float range;
        public float ammoConsumeRate;
        public float aimSpeedMultiplier;
        public float maxAmmo;
        public AudioClip lasergunBeam;
        public AudioClip lasergunFire;
        public Sprite[] Uppersprites;
        public Sprite[] Cannonsprites;
        public Sprite[] Bottomsprites;
        public LayerMask enemyLayerMask;
    }

    public MachineGun machineGun;
    public LaserGun laserGun;
    public MissileGun missileGun;

    public float MaxHealth => _maxHealth;
    public float AimSpeed => _aimSpeed;
}