using Interfaces;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private BasicEnemyData _basicEnemyData = null;
    private ObjectPoolManager _objectPoolManager = null;
    private Vector3 _screenBounds;

    private Vector3 _bulletDirection = Vector3.zero;
    private Vector3 currentPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        _screenBounds = GameManager.GetScreenBounds;
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }
   private void Update()
   {
        currentPos = gameObject.transform.position;
        direction =targetPos - currentPos;
        //Quaternion lookat = Quaternion.LookRotation(direction,Vector3.up);
        //_sprite.transform.rotation = Quaternion.Lerp(transform.rotation, lookat, Time.deltaTime* _basicEnemyData.Basic_AttackSpeed*5.0f);
        //Quaternion lookat = Quaternion.
        direction.Normalize();
        //Vector3 dir = new Vector3(0.0f, 0.0f, direction.z);
        _sprite.transform.eulerAngles =direction;
        
        transform.position += direction * _basicEnemyData.Basic_AttackSpeed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
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

            IDamageable<float> damageable = target.GetComponent<IDamageable<float>>();
            if (damageable != null)
            {
                damageable.TakeDamage(_basicEnemyData.Basic_AttackDamage);
                Debug.Log($"[Collider2D] -- Enemy_Projectile -- {target.gameObject.name}");
                RecycleBullet();
            }
        }
    }
    public void SetTarget(Vector3 tartgetpos)
    {
        targetPos = tartgetpos;


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
