using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private AmmoData _ammoData = default;
    [SerializeField] private ParticleSystem _explosionParticle = default;
    [SerializeField] private LayerMask _layerEnemyMask = default;

    private Vector3 _screenBounds;
    private ObjectPoolManager _objectPoolManager = null;
    public float AreaOfEffect { get; set; } = 50f;

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }

    private void Start()
    {
        _screenBounds = GameManager.GetScreenBounds;
    }

    private void Update()
    {
        if (_ammoData.MoveSpeed != 0f)
        {
            transform.position += transform.up * (_ammoData.MoveSpeed * Time.deltaTime);

            // set activated false prefabs when touch the camera bounds
            if ((transform.position.x >= _screenBounds.x) ||
                (transform.position.x <= -_screenBounds.x) ||
                (transform.position.y >= _screenBounds.y) ||
                (transform.position.y <= -_screenBounds.y))
            {
                RecycleBullet();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Somente teste
        // IDamageable<float> damageable = collision.GetComponentInParent<EnemyHealth>();
        //if (damageable == null) return;
        //damageable.TakeDamage(_ammoData.Damage, _ammoData.Type);


        //TODO: arrumar isso
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
        var colliders = Physics2D.OverlapCircleAll(collision.gameObject.transform.position, AreaOfEffect, _layerEnemyMask);
        foreach (Collider2D enemy in colliders)
        {
            Debug.Log($"[Collider2D] -- {enemy.gameObject.name}");
            IDamageable<float> damageable = enemy.GetComponentInParent<EnemyHealth>();
            if (damageable == null) return;
            if (!_triggerExplosionOnce)
            {
                ParticleSystem particle = Instantiate(_explosionParticle, gameObject.transform.position, Quaternion.identity);
                particle.Play();
                Destroy(particle, particle.main.duration);
                _triggerExplosionOnce = true;
            }

            damageable.TakeDamage(_ammoData.Damage, _ammoData.Type);
        }
        RecycleBullet();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gameObject.transform.position, AreaOfEffect);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, AreaOfEffect);
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