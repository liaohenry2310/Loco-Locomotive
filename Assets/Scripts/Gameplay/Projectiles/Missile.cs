﻿using Interfaces;
using UnityEngine;

public class Missile : MonoBehaviour
{
    //[SerializeField] private MissileData _missileData = default;
    //[SerializeField] private LayerMask _layerEnemyMask = default;
    [SerializeField] private TurretData _turretData = default;
    [SerializeField] private ParticleSystem _explosionParticle = default;

    private Vector3 _screenBounds;
    private float _currentSpeed = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;
    public AudioSource Audio;
    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        Audio = gameObject.AddComponent<AudioSource>();
        Audio.playOnAwake = false;
        Audio.volume = 1f;
    }

    private void Start()
    {
        _screenBounds = GameManager.GetScreenBounds;
        Audio.clip = _turretData.missileGun.missilegunBeam;
    }

    private void FixedUpdate()
    {
        _currentSpeed += Mathf.Lerp(_turretData.missileGun.minSpeed , _turretData.missileGun.maxSpeed, _turretData.missileGun.acceleration * Time.fixedDeltaTime);
        _currentSpeed = Mathf.Clamp(_currentSpeed, _turretData.missileGun.minSpeed, _turretData.missileGun.maxSpeed);
        transform.Translate(transform.up * Time.fixedDeltaTime * _currentSpeed, Space.World);
        
        // set activated false prefabs when touch the camera bounds
        if ((transform.position.x >= _screenBounds.x) ||
            (transform.position.x <= -_screenBounds.x) ||
            (transform.position.y >= _screenBounds.y) ||
            (transform.position.y <= -_screenBounds.y))
        {
            RecycleBullet();
            _currentSpeed = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: arrumar isso pois ta uma bosta
        // Usando o pool
        //explosion = _objectPoolManager.GetObjectFromPool("MissileExplosion");
        //if (!explosion)
        //{
        //    Debug.LogWarning("Bullet Object Pool is Empty");
        //    return;
        //}
        //explosion.transform.position = gameObject.transform.position;
        //explosion.transform.rotation = Quaternion.identity;
        //ParticleSystem p =  explosion.GetComponent<ParticleSystem>();
        //p.Play();

        //TODO: Test here
        MissileExplostion(collision);
    }

    private void MissileExplostion(Collider2D collision)
    {
        bool _triggerExplosionOnce = false;
        var colliders = Physics2D.OverlapCircleAll(collision.gameObject.transform.position, _turretData.missileGun.radiusEffect, _turretData.missileGun.enemyLayerMask);
        foreach (Collider2D enemy in colliders)
        {
            Debug.Log($"[Collider2D] -- MissileExplostion -- {enemy.gameObject.name}");
            IDamageableType<float> damageable = enemy.GetComponent<EnemyHealth>();
            if (damageable == null) return;
            if (!_triggerExplosionOnce)
            {
                ParticleSystem particle = Instantiate(_explosionParticle, gameObject.transform.position, Quaternion.identity);
                particle.Play();
                Audio.Play();
                Destroy(particle, particle.main.duration);
                _triggerExplosionOnce = true;
            }

            damageable.TakeDamage(_turretData.missileGun.damage, DispenserData.Type.Missile);
        }
        if (colliders.Length == 0) return;
        RecycleBullet();
        _currentSpeed = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gameObject.transform.position, _turretData.missileGun.radiusEffect);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, _turretData.missileGun.radiusEffect);
    }


    private void RecycleBullet()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _objectPoolManager.RecycleObject(gameObject);
    }

}