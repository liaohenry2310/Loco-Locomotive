using Interfaces;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private BasicEnemyData _basicEnemyData = null;
    private ObjectPoolManager _objectPoolManager = null;
    private Vector3 _screenBounds;

    private Vector3 _bulletDirection = Vector3.zero;

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        _screenBounds = GameManager.GetScreenBounds;
    }

    private void FixedUpdate()
    {
        transform.position += transform.up * (_basicEnemyData.Basic_AttackSpeed * Time.fixedDeltaTime);
        transform.position += _bulletDirection + transform.up;

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
        Explostion(collision);
    }

    private void Explostion(Collider2D collision)
    {
        var colliders = Physics2D.OverlapCircleAll(collision.gameObject.transform.position, 0.2f);
        foreach (Collider2D target in colliders)
        {
            //TODO: Kairus you can delete this block here.
            //if (target.GetComponent<EnemyHealth>())
            //{
            //    continue;
            //}
            IDamageable<float> damageable = target.GetComponent<IDamageable<float>>();
            if (damageable != null)
            {
                damageable.TakeDamage(_basicEnemyData.Basic_AttackDamage);
                Debug.Log($"[Collider2D] -- MissileExplostion -- {target.gameObject.name}");
            }
            RecycleBullet();
        }
    }
    public void SetFire(Vector3 tartgetpos)
    {
        _bulletDirection = (tartgetpos - transform.position).normalized;
        _bulletDirection = Quaternion.Euler(0.0f, 0.0f, Random.Range(-15.0f, 15.0f)) * _bulletDirection; //Randomize the direction of the bullet a small bit.
    }

    private void RecycleBullet()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _objectPoolManager.RecycleObject(gameObject);
        Debug.Log("RecycleProjectile!");
    }
}
