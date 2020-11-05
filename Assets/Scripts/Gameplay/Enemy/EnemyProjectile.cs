using Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private ObjectPoolManager _objectPoolManager = null;
    [SerializeField] private BasicEnemyData _basicEnemyData = null;
    private Vector3 _screenBounds;

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        _screenBounds = GameManager.GetScreenBounds;
    }
    private void FixedUpdate()
    {
        //transform.position += transform.up * (_basicEnemyData.Basic_AttackSpeed * Time.fixedDeltaTime);

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
        var colliders = Physics2D.OverlapCircleAll(collision.gameObject.transform.position,0.2f);
        foreach (Collider2D target in colliders)
        {
            Debug.Log($"[Collider2D] -- MissileExplostion -- {target.gameObject.name}");

            if (target.GetComponent<EnemyHealth>())
            {
                continue;
            }
            IDamageable<float> damageable = target.GetComponent<IDamageable<float>>();
            if (target != null)
            {
                damageable.TakeDamage(_basicEnemyData.Basic_AttackDamage);
            }
            RecycleBullet();
        }
    }
    public void SetFire(Vector3 tartgetpos)
    {
        Vector3 bulletDir = tartgetpos - transform.position;
        bulletDir.Normalize();

        bulletDir = Quaternion.Euler(0.0f, 0.0f, Random.Range(-15.0f, 15.0f)) * bulletDir; //Randomize the direction of the bullet a small bit.
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
