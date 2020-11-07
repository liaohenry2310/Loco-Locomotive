using Interfaces;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private TurretData _turretData = null;
    [SerializeField] private GameObject _bulletSound = null;
    private Vector3 _screenBounds;
    private ObjectPoolManager _objectPoolManager = null;
    void Start()
    {
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
        IDamageableType<float> damageable = collision.GetComponent<EnemyHealth>();
        if (damageable != null)
        {
            damageable.TakeDamage(_turretData.machineGun.damage, DispenserData.Type.Normal);
        }
        Instantiate(_bulletSound, gameObject.transform.position, Quaternion.identity);
        RecycleBullet();
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
