using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turrets/Turret")]
public class TurretData : ScriptableObject, ISerializationCallbackReceiver
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
        public LayerMask enemyLayerMask;
    }

    [Serializable]
    public struct ShockWave
    {
        public float moveSpeed;
        public float maxSize;
        public float growthDuration;
        public float maxAmmo;
        public float fireRate;
        public float aimSpeedMultiplier;
    }
    
    [Serializable]
    public struct ShieldGun
    {
        public float maxAmmo;
        public float ammoConsumeRate;
    }

    public Sprite[] Bottomsprites;
    public MachineGun machineGun;
    public LaserGun laserGun;
    public MissileGun missileGun;
    public ShockWave empShockWave;
    public ShieldGun shieldGun;

    public float MaxHealth => _maxHealth;
    public float AimSpeed => _aimSpeed;

    [NonSerialized] public float CurrentHealth = 0.0f;

    public void OnBeforeSerialize()
    {
        CurrentHealth = _maxHealth;
    }

    public void OnAfterDeserialize()
    {}

}