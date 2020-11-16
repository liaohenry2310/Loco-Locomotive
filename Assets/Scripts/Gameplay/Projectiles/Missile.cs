using Interfaces;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private TurretData _turretData = null;
    [SerializeField] private ParticleSystem _explosionParticle = null;
    [SerializeField] private GameObject _missileSound = null;

    private Vector3 _screenBounds;
    private float _currentSpeed = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;
    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }

    private void Start()
    {
        _screenBounds = GameManager.GetScreenBounds;
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
        //TODO: 
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
                ParticleSystem.MainModule main = particle.main;
                main.startSize = _turretData.missileGun.radiusEffect;

                Instantiate(_missileSound, gameObject.transform.position, Quaternion.identity);

                particle.Play();
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