using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    //call target dir from list.
    [SerializeField] private TrainData _trainData = null;

    public BasicEnemyData enemyData;

    private Vector3 _velocity;
    private float _maxSpeed;
    private float _speed;
    private float _nextAttackTime = 0.0f;

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private float _currentHealth = 0.0f;
    private float _currentShieldHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;
    private GameObject _projectile;

    private EnemyHealth _healthdata;
    private Vector3 _screenBounds;

    //set random heading, make sure not stick together
    private bool _changeHeading;
    private float _randomHeadingtimer;




    enum Direction
    {
        Idle,
        Left,
        Right
    }
    Direction currentDir = Direction.Idle;

    private bool isAlive = false;




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
        _speed = enemyData.Speed;
        _maxSpeed = enemyData.MaxSpeed;
        gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        gameObject.GetComponent<EnemyHealth>().ReSetHealth = true;
        _projectile = enemyData.projectile;
        _nextAttackTime = Time.time + 1.0f + Random.Range(-enemyData.AttackDelay * 0.8f, enemyData.AttackDelay * 0.8f);
        if (gameObject.CompareTag("ShieldEnemy"))
        {
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ShieldHealth = _currentShieldHealth;
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ReShield = true;
        }
        isAlive = true;
        _velocity = Vector3.zero;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Rigidbody2D rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        rigidbody2D.gravityScale = 0.1f;
        _changeHeading = true;
        _nextAttackTime = 0.0f;
        _randomHeadingtimer = 0.0f;

    }

    private void FixedUpdate()
    {
        if ((transform.position.x >= _screenBounds.x) ||
           (transform.position.x <= -_screenBounds.x) ||
           (transform.position.y >= _screenBounds.y) ||
           (transform.position.y <= -_screenBounds.y))
        {
            RecycleBasicEnemy();
        }
    }
    void Update()
    {

        FlyAndShootUpdate();
        CheckStillAlive();
    }


    void FlyAndShootUpdate()
    {
        //Movement

        Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);

        _acceleration = (Vector3)(BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform,transform.position+_velocity.normalized, _speed), enemyData.SeekBehaviorWeight));
        _acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 3.0f), enemyData.WanderBehaviorWeight));
        if (_changeHeading)
        {
            var xDir = Random.Range(0, 2) == 1 ? -1 : 1;
            var yDir = Random.Range(0, 2) == 1 ? -1 : 1;
           _acceleration.x *= xDir ;
           _acceleration.y *= yDir ;
            _changeHeading = false;
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

        _velocity += _acceleration * Time.deltaTime;
        if (_velocity.sqrMagnitude > _maxSpeed)
        {
            var speed = _velocity.magnitude;
            _velocity.Normalize();
            _velocity /= speed;
            _velocity *= _maxSpeed;
        }
        transform.position += _velocity * Time.deltaTime* _speed;
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
        if (movingDir != currentDir)
        {
            //transform.localRotation = Quaternion.Euler(0, 0, 0.1f*Time.deltaTime);
            currentDir = movingDir;
        }
        else
        {
            //transform.Rotate(0, 0, heading.x * -3.0f*Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(heading.x * -enemyData.Basic_tiltingAngle + (Time.deltaTime * 2.0f), Vector3.forward);
        }

        if (isAlive)
        {
            if (_randomHeadingtimer<Time.time)
            {
                _randomHeadingtimer = Time.time + enemyData.RandomHeadingTimer;
                _changeHeading = true;
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
                projectile.GetComponent<EnemyProjectile>().SetData(targetPos, enemyData.Basic_AttackSpeed,enemyData.Basic_AttackDamage,EnemyTypeCheck.Type.Basic);

            }
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
            Rigidbody2D rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _velocity = Vector3.zero;
            rigidbody2D.gravityScale = 0.5f;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
