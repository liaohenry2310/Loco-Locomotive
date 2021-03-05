using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turrets/Turret")]
public class TurretData : ScriptableObject
{
    [SerializeField] private float _maxHealth = 100.0f;
    [SerializeField, Range(50.0f, 360.0f)] private float _aimSpeed = 55.0f;

    [Header("Tweeking Turret when receive Damage")]
    [SerializeField, Range(0.2f, 1f), Tooltip("How much time spent to shake")] private float _shakeTime = 0.2f;
    [SerializeField, Range(0.05f, 1f), Tooltip("How much force shaking in X direction")] private float _shakeForce = 0.05f;
    [SerializeField] private float _retractitleCannonSpeed = 5.0f;
    [SerializeField] private float _smokeMaxEmission = 10.0f;
    [SerializeField] private TrainData _trainData = null;
    [SerializeField] private float[] _damageMultiplier = new float[4];

    [Serializable]
    public struct MachineGun
    {
        public float damage;
        public float moveSpeed;
        public float fireRate;
        public float maxAmmo;
        public float spreadBullet;
        public float recoilForce;
        public AudioClip machinegunFire;
        //public AudioClip machinegunHit;
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
        public float recoilForce;
        public AudioClip missilegunFire;
        //public AudioClip missilegunHit;
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
        public float recoildForce;
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


    /// <summary>
    /// Count how many players is on the train, accessing by TrainDataSO
    /// </summary>
    public int PlayersOnScene => _trainData.PlayerCount;

    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float AimSpeed { get => _aimSpeed; set => _aimSpeed = value; }

    public float ShakeTime { get => _shakeTime; set => _shakeTime = value; }
    public float ShakeForce { get => _shakeForce; set => _shakeForce = value; }
    public float RetractitleCannonSpeed { get => _retractitleCannonSpeed; set => _retractitleCannonSpeed = value; }
    public float SmokeMaxEmission { get => _smokeMaxEmission; set => _smokeMaxEmission = value; }

    public float[] DamageMulti { get => _damageMultiplier; set => _damageMultiplier = value; }

    public float DamageMultiplier(float damage, int playersNumbers)
    {
        if (playersNumbers - 1 < 0)
            return damage * _damageMultiplier[0];
        if (playersNumbers - 1 > 3)
            return damage * _damageMultiplier[3];
        return damage * _damageMultiplier[playersNumbers - 1];
    }

}