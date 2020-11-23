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
    private GameObject _projectile;




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
        //_objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
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
        _projectile = enemyData.projectile;
        _nextAttackTime = enemyData.AttackDelay+ Time.time;
        isAlive = true;
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
        Vector3 targetPos = targetlist[randomtarget].gameObject.transform.position;

        Vector2 destination = new Vector2(targetPos.x, transform.position.y);
        _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, destination, enemyData.MaxSpeed), enemyData.SeekWeight);
        float dis = float.MaxValue;
        
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
        yield return new WaitForSeconds(5.0f);
        mCurrentState = State.Attack;
    }
    void Attack()
    { }

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

}
