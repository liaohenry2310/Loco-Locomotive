using Interfaces;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private TurretData _turretData = null;
    [SerializeField] private ParticleSystem _hitVFX = null;
    [SerializeField] private GameObject _bulletSound = null;
    private Vector3 _screenBounds;
    private ObjectPoolManager _objectPoolManager = null;

    void Start()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _screenBounds = GameManager.GetScreenBounds;
    }

    private void FixedUpdate()
    {
        transform.position += transform.up * (_turretData.machineGun.moveSpeed * Time.fixedDeltaTime);

        // set activated false prefabs when touch the camera bounds
        if ((transform.position.x >= _screenBounds.x) ||
            (transform.position.x <= -_screenBounds.x) ||
            (transform.position.y >= _screenBounds.y) ||
            (transform.position.y <= -_screenBounds.y))
        {
            RecycleBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyProjectile enemyProjectile = collision.GetComponent<EnemyProjectile>();

        if (enemyProjectile != null && enemyProjectile.CurrenyEnemeyType == EnemyTypeCheck.Type.Bomber)
        {
            ParticleSystem particle = Instantiate(_hitVFX, transform.position, Quaternion.identity);
            particle.Play();
            Destroy(particle, particle.main.duration);
            Instantiate(_bulletSound, gameObject.transform.position, Quaternion.identity);
            enemyProjectile.DestroyBullet = true;
            RecycleBullet();
        }

        IDamageableType<float> damageable = collision.GetComponent<EnemyHealth>();
        if (damageable != null)
        {
            damageable.TakeDamage(_turretData.DamageMultiplier(_turretData.machineGun.damage, _turretData.PlayersOnScene), DispenserData.Type.Normal);
            //For now will work like so, but still call GC.
            ParticleSystem particle = Instantiate(_hitVFX, transform.position, Quaternion.identity);
            particle.Play();
            Destroy(particle, particle.main.duration);
            Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constant);

            // This methods save 200ms in game
            //if (isActiveAndEnabled)
            //{
            //    _bulletVFX.gameObject.transform.position = transform.position;
            //    _bulletVFX.Play();
            //}
            Instantiate(_bulletSound, gameObject.transform.position, Quaternion.identity);
            RecycleBullet();
        }

    }

    private void RecycleBullet()
    {
        _objectPoolManager.RecycleObject(gameObject);
    }

}