using Interfaces;
using System.Collections;
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
        _screenBounds = GameManager.GetScreenBounds;
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
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
        
        
        if (enemyProjectile!=null&& enemyProjectile.CurrenyEnemeyType == EnemyTypeCheck.Type.Bomber)
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
            damageable.TakeDamage(_turretData.machineGun.damage, DispenserData.Type.Normal);
            //StartCoroutine(HitExplosion());
            //For now will work like so
            ParticleSystem particle = Instantiate(_hitVFX, transform.position, Quaternion.identity);
            particle.Play();
            Destroy(particle, particle.main.duration);
            Instantiate(_bulletSound, gameObject.transform.position, Quaternion.identity);
            RecycleBullet();
        }


    }

    //TODO: this functions still in test
    // Need to improve this function to avoid GC
    private IEnumerator HitExplosion()
    {
        GameObject particle = _objectPoolManager.GetObjectFromPool("bulletVFX");
        particle.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        var particles = particle.GetComponent<ParticleSystem>();
        particles.Play();
        yield return new WaitForSeconds(particles.main.duration);
        _objectPoolManager.RecycleObject(particle);
        RecycleBullet();
    }


    private void RecycleBullet()
    {
        _objectPoolManager.RecycleObject(gameObject);
    }
}
