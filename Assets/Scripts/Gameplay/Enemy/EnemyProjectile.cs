using Interfaces;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //[SerializeField] private BasicEnemyData _basicEnemyData = null;
    //[SerializeField] private BasicEnemyData _bomberEnemyData = null;
    [SerializeField] private ParticleSystem _hitVFX = null;
    [SerializeField] private ParticleSystem _explosionVFX = null;
    [SerializeField] private ParticleSystem _VFX = null;

    private ParticleSystem _projectileVFX = null;

    private float _AttackSpeed = 0.0f;
    private float _AttackDamage = 0.0f;

    private ObjectPoolManager _objectPoolManager = null;
    private Vector3 _screenBounds;

    private Vector3 _bulletDirection = Vector3.zero;
    private Vector3 currentPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private SpriteRenderer _sprite;
    private EnemyTypeCheck.Type _currenyEnemyType = EnemyTypeCheck.Type.None;
    public EnemyTypeCheck.Type CurrenyEnemeyType { get { return _currenyEnemyType; } set { _currenyEnemyType = value; } }

    private bool destroyBullet = false;
    //private bool _isBomber = false;

    public bool DestroyBullet { get { return destroyBullet; } set { destroyBullet = value; } }

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _screenBounds = GameManager.GetScreenBounds;
        //if (!_projectileVFX)
        //{
        //    _projectileVFX = Instantiate(_hitVFX, transform.position, Quaternion.identity);
        //}
    }
    
    //private void OnDisable()
    //{
    //    if (_projectileVFX)
    //    {
    //        Destroy(_projectileVFX.gameObject, _projectileVFX.main.duration + 0.5f);
    //    }
    //}

    private void Update()
    {
        if (DestroyBullet)
        {
            RecycleBullet();
            DestroyBullet = false;
        }
    }

    private void FixedUpdate()
    {
        if (_currenyEnemyType == EnemyTypeCheck.Type.Basic)
        {
            currentPos = gameObject.transform.position;
            direction = targetPos - currentPos;
            //Quaternion lookat = Quaternion.LookRotation(direction,Vector3.up);
            //_sprite.transform.rotation = Quaternion.Lerp(transform.rotation, lookat, Time.deltaTime* _basicEnemyData.Basic_AttackSpeed*5.0f);
            //Quaternion lookat = Quaternion.
            direction.Normalize();
            //Vector3 dir = new Vector3(0.0f, 0.0f, direction.z);
            _sprite.transform.eulerAngles = direction;
            transform.position += direction * _AttackSpeed * Time.fixedDeltaTime;
        }
        if (_currenyEnemyType== EnemyTypeCheck.Type.Bomber)
        {
            Rigidbody2D rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            MovingParticle();
            //transform.Rotate(0.0f,0.0f,5.0f*Time.deltaTime);
        }
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

    private void Explostion(Collider2D collider)
    {
        var colliders = Physics2D.OverlapCircleAll(collider.gameObject.transform.position, 0.2f);
        foreach (Collider2D target in colliders)
        {

            IDamageable<float> damageable = target.GetComponent<IDamageable<float>>();
            if (damageable != null)
            {
                damageable.TakeDamage(_AttackDamage);
                //TODO: old way
                ParticleSystem particle = Instantiate(_hitVFX, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constant);
            }
        }
        //if (isActiveAndEnabled)
        //{
        //    _projectileVFX.gameObject.transform.position = transform.position;
        //    _projectileVFX.Play();
        //}

        RecycleBullet();
    }
    public void PlayParticle(Vector3 pos)
    {
        ParticleSystem particle = Instantiate(_explosionVFX, pos, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration);
    }
    public void MovingParticle()
    {
        //ParticleSystem particle = Instantiate(_VFX, new Vector3(transform.position.x-0.5f,transform.position.y-0.5f,transform.position.z), Quaternion.identity);
        ParticleSystem particle = Instantiate(_VFX, transform.position, Quaternion.identity);
        particle.Play();
        Destroy(particle, particle.main.duration);
    }
    public void SetData(Vector3 tartgetpos, float enemyAttackSpeed,float enemyAttackDamage, EnemyTypeCheck.Type enemyType)
    {
        targetPos = tartgetpos;

        _AttackSpeed = enemyAttackSpeed;
        _AttackDamage = enemyAttackDamage;
        _currenyEnemyType = enemyType;
        if (_currenyEnemyType == EnemyTypeCheck.Type.Bomber)
        {
            Rigidbody2D rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void RecycleBullet()
    {
        _objectPoolManager.RecycleObject(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(gameObject.transform.position, 0.2f);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, 0.2f);
    }

}
