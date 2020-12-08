using Interfaces;
using UnityEngine;


public class SwarmEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitVFX = null;

    private SwarmEnemyData _enemyData = null;
    private ObjectPoolManager _objectPoolManager = null;

    private Vector3 _bulletDirection = Vector3.zero;
    private Vector3 currentPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private SpriteRenderer _sprite;
    private Vector3 _screenBounds;

    private float _currentHealth = 0.0f;
    private float _currentShieldHealth = 0.0f;
    private float _oldPos;
    private bool isAilve = false;

    private bool isAttacking = false;
    public bool Alive { get { return isAilve; } set { isAilve = value; } }
    public bool Attacking { get { return isAttacking; } set { isAttacking = value; } }
    private bool fire = false;
    public Vector3 Target { get { return targetPos; } set { targetPos = value; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        _sprite = GetComponent<SpriteRenderer>();
        _screenBounds = GameManager.GetScreenBounds;
    }
    private void Start()
    {
        _oldPos = transform.position.x;
    }
    private void FixedUpdate()
    {

        // set activated false prefabs when touch the camera bounds
        if ((transform.position.x >= _screenBounds.x) ||
            (transform.position.x <= -_screenBounds.x) ||
            (transform.position.y >= _screenBounds.y) ||
            (transform.position.y <= -_screenBounds.y))
        {
            RecycleSwarm();
        }
    }

    private void Update()
    {
        if (_oldPos < transform.position.x)
            _sprite.flipX = true;
        else if (_oldPos > transform.position.x)
            _sprite.flipX = false;
        if (fire)
        {
            currentPos = gameObject.transform.position;
            direction = targetPos - currentPos;
            //Quaternion lookat = Quaternion.LookRotation(direction,Vector3.up);
            //_sprite.transform.rotation = Quaternion.Lerp(transform.rotation, lookat, Time.deltaTime* _basicEnemyData.Basic_AttackSpeed*5.0f);
            //Quaternion look at = Quaternion.
            direction.Normalize();
            //Vector3 dir = new Vector3(0.0f, 0.0f, direction.z);
            _sprite.transform.eulerAngles = direction;
            transform.position += direction * _enemyData.Swarm_AttackSpeed * Time.deltaTime;
        }
        CheckStillAlive();
    }



    public void SetNewData(SwarmEnemyData enemyData)
    {
        _enemyData = enemyData;
        _currentHealth = enemyData.MaxHealth;
        _currentShieldHealth = enemyData.ShieldHealth;
        gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        gameObject.GetComponent<EnemyHealth>().ReSetHealth = true;
        if (gameObject.CompareTag("ShieldEnemy"))
        {
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ShieldHealth = _currentShieldHealth;
        }

        Alive = true;

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
                damageable.TakeDamage(_enemyData.Swarm_AttackDamage);
                ParticleSystem particle = Instantiate(_hitVFX, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(particle, particle.main.duration);
                Alive = false;
                RecycleSwarm();
            }
        }
    }
    public void Setfire(bool gofire)
    {
        fire = gofire;
    }
    public void SetTarget(Vector3 tartgetpos)
    {
        targetPos = tartgetpos;
    }

    private void CheckStillAlive()
    {
        if (!(gameObject.GetComponent<EnemyHealth>().IsAlive()))
        {
            Alive = false;
            RecycleSwarm();
        }

    }
    private void RecycleSwarm()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        Alive = false;
        _objectPoolManager.RecycleObject(gameObject);
    }

}