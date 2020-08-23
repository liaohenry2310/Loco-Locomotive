using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private MissileData _missileData = default;
    [SerializeField] private ParticleSystem _explosionParticle = default;
    [SerializeField] private LayerMask _layerEnemyMask = default;

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
        _currentSpeed += Mathf.Lerp(_missileData.MinSpeed, _missileData.MaxSpeed, _missileData.Acceleration * Time.fixedDeltaTime);
        _currentSpeed = Mathf.Clamp(_currentSpeed, _missileData.MinSpeed, _missileData.MaxSpeed);
        transform.Translate(transform.up * Time.fixedDeltaTime * _currentSpeed, Space.World);
        // bck
        //transform.position += transform.up * (_missileData.MoveSpeed * Time.deltaTime);

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
        // TODO: Somente teste
        // IDamageable<float> damageable = collision.GetComponentInParent<EnemyHealth>();
        //if (damageable == null) return;
        //damageable.TakeDamage(_ammoData.Damage, _ammoData.Type);


        //TODO: arrumar isso poista ta uma bosta
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

        //TODO: testar isso tb
        MissileExplostion(collision);
    }

    private void MissileExplostion(Collider2D collision)
    {
        bool _triggerExplosionOnce = false;
        var colliders = Physics2D.OverlapCircleAll(collision.gameObject.transform.position, _missileData.RadiusEffect, _layerEnemyMask);
        foreach (Collider2D enemy in colliders)
        {
            Debug.Log($"[Collider2D] -- {enemy.gameObject.name}");
            IDamageable<float> damageable = enemy.GetComponent<EnemyHealth>();
            if (damageable == null) return;
            if (!_triggerExplosionOnce)
            {
                ParticleSystem particle = Instantiate(_explosionParticle, gameObject.transform.position, Quaternion.identity);
                particle.Play();
                Destroy(particle, particle.main.duration);
                _triggerExplosionOnce = true;
            }

            damageable.TakeDamage(_missileData.Damage, _missileData.Type);
        }
        if (colliders.Length == 0) return;
        RecycleBullet();
        _currentSpeed = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gameObject.transform.position, _missileData.RadiusEffect);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, _missileData.RadiusEffect);
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