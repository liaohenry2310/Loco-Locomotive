using Interfaces;
using System.Collections;
using UnityEngine;


public class SwarmEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitVFX = null;

    private TrailRenderer trailVFX;
    private SwarmEnemyData _enemyData = null;
    private ObjectPoolManager _objectPoolManager = null;

    private Vector3 _bulletDirection = Vector3.zero;
    private Vector3 currentPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 bouncedir;
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

    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay = 0.02f;
    public float shake_intensity = 0.03f;
    private float temp_shake_intensity = 0;

    public AudioClip clip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _screenBounds = GameManager.GetScreenBounds;
        trailVFX = gameObject.GetComponent<TrailRenderer>();
        if (!TryGetComponent(out _audioSource))
        {
            Debug.LogWarning("Fail to load Audio Source component.");
        }
    }

    private void Start()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _oldPos = transform.position.x;
        _audioSource.volume = 0.1f;
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

        CheckStillAlive();

    }

    public void Shake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        temp_shake_intensity = shake_intensity;
        StartCoroutine(StartShake());
    }
    private IEnumerator StartShake()
    {
        transform.position = originPosition + Random.insideUnitSphere * 0.09f;
        yield return new WaitForSeconds(0.5f);
    }

    public void SetNewData(SwarmEnemyData enemyData)
    {
        _enemyData = enemyData;
        _currentHealth = enemyData.MaxHealth;
        _currentShieldHealth = enemyData.ShieldHealth;
        Velocity = Vector3.zero;
        gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        gameObject.GetComponent<EnemyHealth>().ReSetHealth = true;
        if (gameObject.CompareTag("ShieldEnemy"))
        {
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ShieldHealth = _currentShieldHealth;
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ReShield = true;
        }

        Alive = true;
        bouncedir = Vector3.zero;
        //trailVFX.enabled = false;
        //gameObject.GetComponent<TrailRenderer>().enabled = false;
        //gameObject.GetComponent<TrailRenderer>().emitting = false;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Alive)
        {
            Explostion(collision);
            bouncedir = collision.transform.position;
        }
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

                Destroy(particle.gameObject, particle.main.duration);
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
            //Swarm Dead Audio
            _audioSource.PlayOneShot(clip);

            var dir = Vector3.Reflect(Velocity.normalized, bouncedir);
            if (dir.y < 0.0f)
            {
                dir.y *= -1;
            }
            if (dir.sqrMagnitude > 3.0f)
            {
                var speed = dir.magnitude;
                dir.Normalize();
                //dir /= (speed);
                dir *= 10.0f;
            }
            else
            {
                dir *= 10.0f;
            }
            transform.position += dir * Time.deltaTime;
        }

    }
    private void RecycleSwarm()
    {
        Alive = false;
        gameObject.GetComponent<EnemyHealth>().DefaulSpriteColor();
        _objectPoolManager.RecycleObject(gameObject);
    }

}