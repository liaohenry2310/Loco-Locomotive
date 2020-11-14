using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    //call target dir from list.
    [SerializeField] private TrainData _trainData = null;   

    public BomberEnemyData enemyData;

    private Vector3 _velocity;
    private float _nextAttackTime = 0.0f;

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private float _currentHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;
    private GameObject _projectile;

    private bool _attackMode = false;

    private Vector3 targetPos;
    private Vector3 direction;

    private Turret.TurretBase currentTarget;




    private bool isAlive=false;



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
        _nextAttackTime = enemyData.AttackDelay;
        isAlive = true;
    }

    void Update()
    {

        if (!_attackMode)
        {
            FlyAndShootUpdate();
        }
        CheckStillAlive();
        if (_nextAttackTime < Time.time)
        {
            _attackMode = true;
            GetTarget();
        }
        if (_attackMode)
        {
            if (_attackMode)
            {
                Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);
                _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(this.transform, new Vector2(currentTarget.transform.position.x, this.transform.position.y), enemyData.MaxSpeed), enemyData.SeekWeight);
                _velocity += _acceleration * Time.deltaTime;
                transform.position += _velocity * Time.deltaTime;
            }
            if (transform.position.x == currentTarget.transform.position.x)
         {
             Fire();
         }
        }
        //if (_currentHealth < 0.0f)
        //{
        //    //disable this enemy and give it back to the object pool.
        //}
    }

    void FlyAndShootUpdate()
    {
        Vector3 _acceleration = new Vector3( 0.0f, 0.0f, 0.0f);

            //Movement
            //_acceleration += WanderBehavior.Calculate(gameobject, weight);
            _acceleration = BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 1.0f),enemyData.WanderBehaviorWeight);
            //_acceleration += WallAvoidance.Calculate(gameobject, weight);
            //_acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WallAvoidance.WallAvoidanceCalculation(transform,_botLeftBound.position.x,_topRightBound.position.x,_topRightBound.position.y,_botLeftBound.position.y),enemyData.WallAvoidWeight));
            _velocity += _acceleration * Time.deltaTime;

            if (  _velocity.magnitude > enemyData.MaxSpeed)
            {
                _velocity.Normalize();
                _velocity *= enemyData.MaxSpeed;
            }

            if (Mathf.Abs(transform.position.x - _botLeftBound.position.x) < 1.0f)
            {
                _velocity = new Vector3(-_velocity.x, _velocity.y, _velocity.z);
            }
            if (Mathf.Abs(transform.position.x - _topRightBound.position.x) < 1.0f)
            {
                _velocity = new Vector3(-_velocity.x, _velocity.y, _velocity.z);
            }
            if (Mathf.Abs(transform.position.y - _topRightBound.position.y) < 1.0f)
            {
                _velocity = new Vector3(_velocity.x, -_velocity.y, _velocity.z);
            }
            if (Mathf.Abs(transform.position.y - _botLeftBound.position.y) < 1.0f)
            {
                _velocity = new Vector3(_velocity.x, -_velocity.y, _velocity.z);
            }

            transform.position += _velocity * Time.deltaTime;

    }
    void GetTarget()
    {
        if (_attackMode)
        {
            float distance = float.MaxValue;
            targetPos = (transform.position);
            var targetlist = _trainData.TurretList;
            currentTarget = null;

            foreach (var target in targetlist)
            {
                if (target != null)
                {
                    if (Vector2.Distance(transform.position, target.transform.position) < distance)
                    {
                        targetPos = (target.transform.position);
                        distance = Vector2.Distance(transform.position, target.transform.position);
                        currentTarget = target;
                    }
                }
            }

        }

    }
    void Fire  ()
    {

        GameObject projectile = _objectPoolManager.GetObjectFromPool("BasicEnemy_Projectile");
        projectile.transform.position = transform.position;
        Vector3 targetPos = currentTarget.gameObject.transform.position;
        projectile.SetActive(true);
        projectile.GetComponent<EnemyProjectile>().SetTarget(targetPos);
        _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);
        _attackMode = false;

    }
    private void RecycleBasicEnemy()
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
            RecycleBasicEnemy();
        }
        
    }

}
