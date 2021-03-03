using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : MonoBehaviour
{
    //call target dir from list.
    [SerializeField] private TrainData _trainData = null;
    [SerializeField] private GameObject _laserHitVFX = null;

    public GiantEnemyData enemyData;
    public SpriteRenderer sr;
    public AudioSource Audio;
    public AudioClip[] clip;

    public AudioClip deadclip;
    private AudioSource _audioSource;

    private float _transparency = 0.0f;
    private Vector3 _scale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 _velocity;
    private float _nextAttackTime = 0.0f;
    private float _chargeTime;
    private float _beamDamage;
    private float _beamDuration;

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private float _currentHealth = 0.0f;
    private float _currentShieldHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;

    Vector3 targetPos = Vector3.zero;
    bool targetSelect = false;

    public LineRenderer lineRenderer;
    private Transform firePoint;
    public GameObject VFX;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    private List<ParticleSystem> _LaserHitVFXList = new List<ParticleSystem>();
    public LayerMask trainLayer;

    private Vector3 _screenBounds;
    Vector3 _acceleration =Vector3.zero;

    //bool isAttacking = false;
    float attackCount = 0.0f;
    float attackDelay = 0.0f;

    float chargeingCount = 0.0f;

    private bool isAlive = false;

    //set random heading, make sure not stick together
    private bool _changeHeading;
    private float _randomHeadingtimer;

    public enum State
    {
        WanderIdle,
        MoveToTarget,
        Charging,
        Attack
    }
    private State mCurrentState = State.WanderIdle;

    private void Awake()
    {
        if (!TryGetComponent(out _audioSource))
        {
            Debug.LogWarning("Fail to load Audio Source component.");
        }

    }

    private void Start()
    {
        _audioSource.volume = 0.16f;
    }

    private void OnEnable()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        _screenBounds = GameManager.GetScreenBounds;
    }

    public void SetNewData(Transform topRight, Transform bottomLeft)
    {
        //Reset all relevant game play data so it can be used again when received by the object pool.
        _topRightBound = topRight;
        _botLeftBound = bottomLeft;
        _currentHealth = enemyData.MaxHealth;
        _currentShieldHealth = enemyData.ShieldHealth;
        mCurrentState = State.WanderIdle;
        gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        gameObject.GetComponent<EnemyHealth>().ReSetHealth = true;
        _nextAttackTime = Time.time + 1.0f + Random.Range(-enemyData.AttackDelay * 0.8f, enemyData.AttackDelay * 0.8f);
        _chargeTime = enemyData.ChargeTime;
        _beamDamage = enemyData.BeamDamage;
        _beamDuration = enemyData.BeamDuration;
        if (gameObject.CompareTag("ShieldEnemy"))
        {
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ShieldHealth = _currentShieldHealth;
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ReShield = true;
        }
        isAlive = true;
        _transparency = 1.0f;
        FillLists();
        DisableLaser();
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        _acceleration = Vector3.zero;
        sr.transform.localScale = _scale;
        targetSelect = false;
        //StopParticles();
    }

    private void FixedUpdate()
    {
        if ((transform.position.x >= _screenBounds.x) ||
           (transform.position.x <= -_screenBounds.x) ||
           (transform.position.y >= _screenBounds.y) ||
           (transform.position.y <= -_screenBounds.y))
        {
            RecycleGiantEnemy();
        }
    }

    void Update()
    {
        sr.transform.Rotate(Vector3.back, 45 * 1.0f * Time.deltaTime, Space.Self);
        switch (mCurrentState)
        {
            case State.WanderIdle:
                if (isAlive)
                {
                    WanderIdle();
                }
                break;
            case State.MoveToTarget:
                if (isAlive)
                {
                    MoveToTarget();
                }
                break;
            case State.Charging:
                if (isAlive)
                {
                    Charging();
                }
                break;
            case State.Attack:
                if (isAlive)
                {
                    Attack();
                }
                break;
            default:
                break;
        }
        CheckStillAlive();
        //if (_transparency<0.0f&& !isAlive)
        //{
        //    Debug.Log("rec Giant");
        //    RecycleGiantEnemy();
        //}
    }

    void WanderIdle()
    {
        //Movement
       _acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, transform.position + _velocity.normalized, enemyData.MaxSpeed), enemyData.SeekWeight*3.0f));
        _acceleration = (Vector3)(BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 3.0f), enemyData.WanderBehaviorWeight));
        if (_changeHeading)
        {
            var xDir = Random.Range(0, 2) == 1 ? -1 : 1;
            var yDir = Random.Range(0, 2) == 1 ? -1 : 1;
            _acceleration.x *= xDir;
            _acceleration.y *= yDir;
            _changeHeading = false;
        }
        _velocity += _acceleration * Time.deltaTime;

        if (_velocity.magnitude > enemyData.MaxSpeed)
        {
            var speed = _velocity.magnitude;
            _velocity.Normalize();
            _velocity /= speed;
            _velocity *= enemyData.MaxSpeed;
        }

        if (transform.position.x < _botLeftBound.position.x)
        {
            _velocity.x *= -1;
        }
        if (transform.position.x > _topRightBound.position.x)
        {
            _velocity.x *= -1;
        }
        if (transform.position.y < _topRightBound.position.y)
        {
            _velocity.y *= -1;
        }
        if (transform.position.y > _botLeftBound.position.y)
        {
            _velocity.y *= -1;
        }


        //if (isAlive)
        //{
            if (_randomHeadingtimer < Time.time)
            {
                _randomHeadingtimer = Time.time + enemyData.RandomHeadingTimer;
                _changeHeading = true;
            }
            transform.position += _velocity * Time.deltaTime*(enemyData.MaxSpeed/10);
            //Shooting
            if (_nextAttackTime < Time.time)
            {
                mCurrentState = State.MoveToTarget;
                // _nextAttackTime = enemyData.AttackDelay + Time.time;
            }
        //}
    }
    int RandomTarget(Turret.TurretBase[] targetlist)
    {
        int targetSize = targetlist.Length;
        int randomtarget = 0;
        if (targetSize ==2)
        {
            randomtarget = Random.Range(1, 10);
            if (randomtarget % 2 == 0)
            {
                randomtarget = 0;
            }
            else
            {
                randomtarget = 1;
            }
        }
        else
        {
            randomtarget = Random.Range(0, targetSize);
        }
        return randomtarget;
    }
    void MoveToTarget()
    {
        Vector3 _acceleration = Vector3.zero;
        var targetlist = _trainData.ListTurret;
        if (!targetSelect)
        {
            var targetNum = RandomTarget(targetlist);
            targetPos = targetlist[targetNum].gameObject.transform.position;
            targetSelect = true;
        }
        Vector2 destination = new Vector2(targetPos.x, transform.position.y);
        _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, destination, enemyData.MaxSpeed), enemyData.SeekWeight);
        _velocity += _acceleration * Time.deltaTime * (enemyData.MaxSpeed / 10);
        if (transform.position.x < _botLeftBound.position.x)
        {
            _velocity.x *= -1;
        }
        if (transform.position.x > _topRightBound.position.x)
        {
            _velocity.x *= -1;
        }
        if (transform.position.y < _topRightBound.position.y)
        {
            _velocity.y *= -1;
        }
        if (transform.position.y > _botLeftBound.position.y)
        {
            _velocity.y *= -1;
        }
        if (Mathf.Abs(transform.position.x - destination.x) < 0.1f)
        {
            mCurrentState = State.Charging;
            _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);
        }
        else
        {
            transform.position += _velocity * Time.deltaTime * (enemyData.MaxSpeed / 10);
        }
    }
    private void Charging()
    {
        sr.transform.Rotate(Vector3.back, 45 * 5.0f * Time.deltaTime, Space.Self);

        chargeingCount += Time.deltaTime;
        VFX.transform.position = (Vector2)transform.position;
        PlayParticles();
        //giant charging 
        Audio.PlayOneShot(clip[0]);
        if (chargeingCount >= _chargeTime)
        {
            chargeingCount = 0.0f;
            mCurrentState = State.Attack;
            Audio.Stop();
        }
    }
    private void Attack()
    {
        //isAttacking = true;
        attackCount += Time.deltaTime;
        //while (attackCount < _beamDuration  )
        if (attackDelay < Time.time)
        {

            lineRenderer.enabled = true;

            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1.0f));
            //start attack audio
            var pos = (Vector2)targetPos;

            lineRenderer.SetPosition(1, pos);
            DisableLaserHitVFX();
            var dir = pos - (Vector2)transform.position;
            RaycastHit2D[] hit = Physics2D.RaycastAll((Vector2)transform.position, dir.normalized, 100.0f, trainLayer);
            //if (hit)
            if (hit != null)
            {
                EnableLaserHitVFX();
                //attack audio
                Audio.clip = clip[1];
                Audio.Play();
                for (int i = 0; i < hit.Length; i++)
                {
                    Collider2D collider = hit[i].collider;
                    if (collider)
                    {
                        IDamageable<float> damageable = collider.GetComponent<IDamageable<float>>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(_beamDamage);
                            lineRenderer.SetPosition(1, hit[i].point);
                            _laserHitVFX.transform.position = lineRenderer.GetPosition(1);
                        }
                    }
                }
            }
            attackDelay = Time.time + 1.0f;

        }


            if (attackCount >= _beamDuration)
           {
               //isAttacking = false;
               Audio.Stop();
               attackCount = 0.0f;
               _nextAttackTime = enemyData.AttackDelay + Time.time;
               StopParticles();
               DisableLaser();
               DisableLaserHitVFX();
               mCurrentState = State.WanderIdle;
           }
        targetSelect = false;
    }

    private void RecycleGiantEnemy()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();

        }
        _objectPoolManager.RecycleObject(gameObject);
    }


    private void CheckStillAlive()
    {
        if (!(gameObject.GetComponent<EnemyHealth>().IsAlive()))
        {
            isAlive = false;
            //isAttacking = false;
            attackCount = 0.0f;
            StopParticles();
            DisableLaser();
            DisableLaserHitVFX();
            //Giant dead audio
            _audioSource.PlayOneShot(deadclip);

            mCurrentState = State.WanderIdle;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            sr.transform.Rotate(Vector3.forward, 45 * 5.0f * Time.deltaTime, Space.Self);
            StartCoroutine("TransparencySR");
            _changeHeading = true;
            //RecycleGiantEnemy();
        }

    }
    void PlayParticles()
    {
        for (int i = 0; i < particles.Count; ++i)
        {
            if (!particles[i].isPlaying)
            {
                particles[i].Play();
            }
        }
    }
    void StopParticles()
    {
        for (int i = 0; i < particles.Count; ++i)
        {
            particles[i].Stop();
        }
    }
    void EnableLaser()
    {
        lineRenderer.enabled = true;
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }


    void FillLists()
    {
        for (int i = 0; i < VFX.transform.childCount; ++i)
        {
            var ps = VFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }

        for (int i = 0; i < _laserHitVFX.transform.childCount; ++i)
        {
            var ps = _laserHitVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps)
            {
                _LaserHitVFXList.Add(ps);
            }
        }
    }
    private IEnumerator TransparencySR()
    {
        _transparency -= 0.25f * Time.deltaTime;
        sr.transform.localScale = new Vector3(_transparency, _transparency, 1.0f);
        sr.color = new Color(0.25f, 0.25f, 0.50f, _transparency);
        yield return new WaitForSeconds(4.0f);
        RecycleGiantEnemy();
    }

    private void DisableLaserHitVFX()
    {
        for (int i = 0; i < _LaserHitVFXList.Count; ++i)
        {
            _LaserHitVFXList[i].Stop();
        }
    }

    private void EnableLaserHitVFX()
    {
        for (int i = 0; i < _LaserHitVFXList.Count; ++i)
        {
            _LaserHitVFXList[i].Play();
        }
    }


}
