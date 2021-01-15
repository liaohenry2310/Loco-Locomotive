using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    //call target dir from list.
    [SerializeField] private TrainData _trainData = null;   

    public BasicEnemyData enemyData;

    private Vector3 _velocity;
    private float _nextAttackTime = 0.0f;

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private float _currentHealth = 0.0f;
    private float _currentShieldHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;
    private GameObject _projectile;

    private EnemyHealth _healthdata;

    enum Direction
    {
        Idle,
        Left,
        Right
    }
    Direction currentDir = Direction.Idle;

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
        _currentShieldHealth = enemyData.ShieldHealth;
        gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        gameObject.GetComponent<EnemyHealth>().ReSetHealth = true;
        _projectile = enemyData.projectile;
        _nextAttackTime = Time.time + 1.0f+Random.Range(-enemyData.AttackDelay * 0.8f, enemyData.AttackDelay * 0.8f);
        if (gameObject.CompareTag("ShieldEnemy"))
        {
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ShieldHealth = _currentShieldHealth;
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ReShield = true;
        }
        isAlive = true;
        _velocity =Vector3.zero;
    }

    void Update()
    {

        FlyAndShootUpdate();
        CheckStillAlive();
        //if (_currentHealth < 0.0f)
        //{
        //    //disable this enemy and give it back to the object pool.
        //}
    }

    void FlyAndShootUpdate()
    {
        //Movement
        Vector3 _acceleration = new Vector3( 0.0f, 0.0f, 0.0f);
        //_acceleration += WanderBehavior.Calculate(gameobject, weight);
        _acceleration = BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 3.0f),enemyData.WanderBehaviorWeight);
        //_acceleration += WallAvoidance.Calculate(gameobject, weight);
        //_acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WallAvoidance.WallAvoidanceCalculation(transform,_botLeftBound.position.x,_topRightBound.position.x,_topRightBound.position.y,_botLeftBound.position.y),enemyData.WallAvoidWeight));
        _velocity += _acceleration * Time.deltaTime;

        if (  _velocity.sqrMagnitude > enemyData.MaxSpeed)
        {
            var speed = _velocity.magnitude;
            _velocity.Normalize();
            _velocity /= speed;
            _velocity *=enemyData.MaxSpeed;
        }

         //if (Mathf.Abs(transform.position.x - _botLeftBound.position.x) > 1.0f)
         if (transform.position.x < _botLeftBound.position.x)
         {
             //_velocity = new Vector3(-_velocity.x, _velocity.y, _velocity.z);
             _velocity.x *= -1;
         }
         //if (Mathf.Abs(transform.position.x - _topRightBound.position.x) > 1.0f)
         if (transform.position.x > _topRightBound.position.x)
         {
             //_velocity = new Vector3(-_velocity.x, _velocity.y, _velocity.z);
             _velocity.x *= -1;
         }
         //if (Mathf.Abs(transform.position.y - _topRightBound.position.y) > 1.0f)
         if (transform.position.y < _topRightBound.position.y)
         {
             //_velocity = new Vector3(_velocity.x, -_velocity.y, _velocity.z);
             _velocity.y *= -1;
         }
         //if (Mathf.Abs(transform.position.y - _botLeftBound.position.y) > 1.0f)
         if (transform.position.y > _botLeftBound.position.y)
         {
             //_velocity = new Vector3(_velocity.x, -_velocity.y, _velocity.z);
             _velocity.y *= -1;
         }


        transform.position += _velocity * Time.deltaTime;
        var heading = _velocity.normalized;
        Direction movingDir;
        if (heading.x < 0.0f)
        {
            movingDir = Direction.Left;
        }
        else
        {
            movingDir = Direction.Right;
        }
        if (movingDir!=currentDir)
        {
            //transform.localRotation = Quaternion.Euler(0, 0, 0.1f*Time.deltaTime);
            currentDir = movingDir;
        }
        else
        {
            //transform.Rotate(0, 0, heading.x * -3.0f*Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(heading.x * -enemyData.Basic_tiltingAngle + (Time.deltaTime * 2.0f), Vector3.forward);
        }

        //Shooting
        if (_nextAttackTime < Time.time)
        {
            var targetlist = _trainData.ListTurret;
            int targetSize = targetlist.Length;
            int randomtarget = Random.Range(0, targetSize);
            _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.18f, enemyData.AttackDelay * 0.18f);

            GameObject projectile = _objectPoolManager.GetObjectFromPool("BasicEnemy_Projectile");
            projectile.transform.position = transform.position;
            Vector3 targetPos = targetlist[randomtarget].gameObject.transform.position;
            projectile.SetActive(true);
            projectile.GetComponent<EnemyProjectile>().SetData(targetPos,EnemyTypeCheck.Type.Basic);
            

        }
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
