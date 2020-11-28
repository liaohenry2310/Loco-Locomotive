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

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private float _currentHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;

    Vector3 targetPos= Vector3.zero;

    public LineRenderer lineRenderer;
    private Transform firePoint;
    public GameObject VFX;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    public LayerMask trainLayer;


    private bool isAlive=false;

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
        _nextAttackTime = enemyData.AttackDelay+ Time.time;
        isAlive = true;
        FillLists();
        DisableLaser();
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
                StartCoroutine(Charging(enemyData.ChargeTime));
                break;
            case State.Attack:
                Attack();
                //EnableLaser();
                break;
            default:
                break;
        }
    }

    void WanderIdle()
    {
        //Movement
        Vector3 _acceleration = new Vector3( 0.0f, 0.0f, 0.0f);
        _acceleration = BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter,1.0f),enemyData.WanderBehaviorWeight);
        _velocity += _acceleration * Time.deltaTime;

        if (  _velocity.magnitude > enemyData.MaxSpeed)
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

        }
    }
    void MoveToTarget()
    {

        Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);

        var targetlist = _trainData.ListTurret;
        int targetSize = targetlist.Length;
        int randomtarget = Random.Range(0, targetSize - 1);
        targetPos = targetlist[randomtarget].gameObject.transform.position;

        Vector2 destination = new Vector2(targetPos.x, transform.position.y);
        _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, destination, enemyData.MaxSpeed), enemyData.SeekWeight);
        float dis = float.MinValue;
        
        if (Mathf.Abs(transform.position.y - destination.y)>dis)
        {
            Debug.Log("stop");
          transform.position += _velocity * Time.deltaTime;
        }
        _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);
        mCurrentState = State.Charging;
    }
    public IEnumerator Charging(float chargingTime)
    {

        for (int i = 0; i < particles.Count; ++i)
        {
            particles[i].Play();
        }
        yield return new WaitForSeconds(5.0f);
        mCurrentState = State.Attack;
    }
    void Attack()
    {
        lineRenderer.enabled = true;
       
        lineRenderer.SetPosition(0, transform.position);
        VFX.transform.position = (Vector2)transform.position;
        var pos = (Vector2)targetPos;

        lineRenderer.SetPosition(1, pos);
        var targetlist = _trainData.ListTurret;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, pos.normalized, pos.magnitude,trainLayer);
        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            Collider2D collider = hit.collider;
            if (collider)
            {
                IDamageable<float> damageable = collider.GetComponent<IDamageable<float>>();
                if (damageable != null)
                {
                    damageable.TakeDamage(enemyData.BeamDamage);
                }
            }
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

    void EnableLaser()
    {
        lineRenderer.enabled = true;
        for (int i = 0; i < particles.Count; ++i)
        {
            particles[i].Play();
        }

    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
        for (int i = 0; i < particles.Count; ++i)
        {
            particles[i].Stop();
        }
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
