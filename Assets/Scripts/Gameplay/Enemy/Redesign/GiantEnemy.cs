using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : MonoBehaviour
{
    //call target dir from list.
    [SerializeField] private TrainData _trainData = null;

    public GiantEnemyData enemyData;

    private Vector3 _velocity;
    private float _nextAttackTime = 0.0f;
    public float _chargeTime;
    public float _beamDamage;
    public float _beamDuration;

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private float _currentHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;

    Vector3 targetPos = Vector3.zero;

    public LineRenderer lineRenderer;
    private Transform firePoint;
    public GameObject VFX;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    public LayerMask trainLayer;

    bool isAttacking = false;
    float attackCount = 0.0f;
    float attackDelay = 0.0f;

    float chargeingCount = 0.0f;
    float chargeingDelay = 0.0f;

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
        gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        _nextAttackTime = enemyData.AttackDelay + Time.time;
        _chargeTime = enemyData.ChargeTime;
        _beamDamage = enemyData.BeamDamage;
        _beamDuration = enemyData.BeamDuration;
        isAlive = true;
        FillLists();
        DisableLaser();
        //StopParticles();
    }

    void Update()
    {
        Debug.Log($"Current State:{mCurrentState}");
        switch (mCurrentState)
        {
            case State.WanderIdle:
                WanderIdle();
                break;
            case State.MoveToTarget:
                MoveToTarget();
                break;
            case State.Charging:
               Charging();
                break;
            case State.Attack:
               // if (!isAttacking)
               // {
                    Attack();
                //}

                //EnableLaser();
                break;
            default:
                break;
        }
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

        if (transform.position.x > _botLeftBound.position.x)
        {
            _velocity.x *= -1;
        }
        if (transform.position.x < _topRightBound.position.x)
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


        transform.position += _velocity * Time.deltaTime;


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
        int randomtarget = Random.Range(0, targetSize );
        targetPos = targetlist[randomtarget].gameObject.transform.position;

        Vector2 destination = new Vector2(targetPos.x, transform.position.y);
        _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, destination, enemyData.MaxSpeed), enemyData.SeekWeight);
        _velocity += _acceleration * Time.deltaTime;
        float dis = float.MinValue;

        if (Mathf.Abs(transform.position.x - destination.x) < 0.1f)
        {
            Debug.Log("stop");
            mCurrentState = State.Charging;
            _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);
        }
        else
        {
            transform.position += _velocity * Time.deltaTime;
        }
    }
    private void Charging()
    {
        chargeingCount += Time.deltaTime;
        VFX.transform.position = (Vector2)transform.position;

        PlayParticles();
        if (chargeingCount >= _chargeTime)
        {
            StopParticles();
            chargeingCount = 0.0f;
            mCurrentState = State.Attack;   
        }
    }
    private void Attack()
    {
        isAttacking = true;
        attackCount += Time.deltaTime;
        //while (attackCount < _beamDuration  )
        if (attackDelay < Time.time )
        {

            lineRenderer.enabled = true;

            lineRenderer.SetPosition(0, transform.position);

            var pos = (Vector2)targetPos;

            lineRenderer.SetPosition(1, pos);
            var dir = pos - (Vector2)transform.position;
            RaycastHit2D[] hit = Physics2D.RaycastAll((Vector2)transform.position, dir.normalized, 100.0f, trainLayer);
            //if (hit)
            if (hit != null)
            {

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
           Debug.Log($"Laser Attack ...");

        }
        if (attackCount >=_beamDuration )
        {
            Debug.Log($"Exit Attack");
            isAttacking = false;
            attackCount = 0.0f;
            _nextAttackTime = enemyData.AttackDelay + Time.time;
            DisableLaser();
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
            RecycleGiantEnemy();
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
    }


}
