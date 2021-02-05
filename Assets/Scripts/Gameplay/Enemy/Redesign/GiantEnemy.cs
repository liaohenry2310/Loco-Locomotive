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
    private float _transparency = 0.0f;
    private Vector3 _scale = new Vector3(1.0f,1.0f,1.0f);
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

    public LineRenderer lineRenderer;
    private Transform firePoint;
    public GameObject VFX;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    private List<ParticleSystem> _LaserHitVFXList = new List<ParticleSystem>();
    public LayerMask trainLayer;


    bool isAttacking = false;
    float attackCount = 0.0f;
    float attackDelay = 0.0f;

    float chargeingCount = 0.0f;

    private bool isAlive = false;

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

    }

    private void OnEnable()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
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
        _nextAttackTime = enemyData.AttackDelay + Time.time;
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
        sr.transform.localScale = _scale;
        //StopParticles();
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
        Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);
        _acceleration = BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 1.0f), enemyData.WanderBehaviorWeight);
        _velocity += _acceleration * Time.deltaTime;

        if (_velocity.magnitude > enemyData.MaxSpeed)
        {
            _velocity.Normalize();
            _velocity *= enemyData.MaxSpeed;
        }

        if (transform.position.x > _botLeftBound.position.x + 1.0f)
        {
            _velocity.x *= -1;
        }
        if (transform.position.x < _topRightBound.position.x - 1.0f)
        {
            _velocity.x *= -1;
        }
        if (transform.position.y < _topRightBound.position.y - 2.0f)
        {
            _velocity.y *= -1;
        }
        if (transform.position.y > _botLeftBound.position.y + 2.0f)
        {
            _velocity.y *= -1;
        }


        transform.position += _velocity * Time.deltaTime*(enemyData.MaxSpeed/10);


        //Shooting
        if (_nextAttackTime < Time.time)
        {
            mCurrentState = State.MoveToTarget;
            // _nextAttackTime = enemyData.AttackDelay + Time.time;
        }
    }
    void MoveToTarget()
    {

        Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);

        var targetlist = _trainData.ListTurret;
        int targetSize = targetlist.Length;
        int randomtarget = Random.Range(0, targetSize);
        targetPos = targetlist[randomtarget].gameObject.transform.position;

        Vector2 destination = new Vector2(targetPos.x, transform.position.y);
        _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, destination, enemyData.MaxSpeed), enemyData.SeekWeight);
        _velocity += _acceleration * Time.deltaTime * (enemyData.MaxSpeed / 10);
        float dis = float.MinValue;

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
        if (chargeingCount >= _chargeTime)
        {

            chargeingCount = 0.0f;
            mCurrentState = State.Attack;
        }
    }
    private void Attack()
    {
        isAttacking = true;
        attackCount += Time.deltaTime;
        //while (attackCount < _beamDuration  )
        if (attackDelay < Time.time)
        {

            lineRenderer.enabled = true;

            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1.0f));

            var pos = (Vector2)targetPos;

            lineRenderer.SetPosition(1, pos);
            DisableLaserHitVFX();
            var dir = pos - (Vector2)transform.position;
            RaycastHit2D[] hit = Physics2D.RaycastAll((Vector2)transform.position, dir.normalized, 100.0f, trainLayer);
            //if (hit)
            if (hit != null)
            {
                EnableLaserHitVFX();
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
                    //Collider2D collider = hit.collider;
                    //if (collider)
                    //{
                    //
                    //    IDamageable<float> damageable = collider.GetComponent<IDamageable<float>>();
                    //    if (damageable != null)
                    //    {
                    //        damageable.TakeDamage(enemyData.BeamDamage);
                    //         lineRenderer.SetPosition(1, hit.point);
                    //    }
                    //}

                }
            }
            attackDelay = Time.time + 1.0f;

        }

            if (attackCount >= _beamDuration)
           {
               isAttacking = false;
               attackCount = 0.0f;
               _nextAttackTime = enemyData.AttackDelay + Time.time;
               StopParticles();
               DisableLaser();
               DisableLaserHitVFX();
               mCurrentState = State.WanderIdle;
           }

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
            isAttacking = false;
            attackCount = 0.0f;
            StopParticles();
            DisableLaser();
            DisableLaserHitVFX();
            mCurrentState = State.WanderIdle;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            sr.transform.Rotate(Vector3.forward, 45 * 5.0f * Time.deltaTime, Space.Self);
            StartCoroutine("TransparencySR");
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
        _transparency -= 0.25f* Time.deltaTime;
        //_scale -= 0.1 * Time.deltaTime;
        sr.transform.localScale = new Vector3(_transparency, _transparency, 1.0f);
        sr.color = new Color(0.25f, 0.25f, 0.50f, _transparency);// * Time.deltaTime);
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
