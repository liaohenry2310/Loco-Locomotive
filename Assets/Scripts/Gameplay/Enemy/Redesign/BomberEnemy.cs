using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    //call target dir from list.
    [SerializeField] private TrainData _trainData = null;

    public BomberEnemyData enemyData;
    public Animator animator;
    public GameObject boom;
    private Vector3 _velocity;
    private float _maxSpeed;
    //private float _speed;
    private float _nextAttackTime = 0.0f;

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private float _currentHealth = 0.0f;
    private float _currentShieldHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;
    private GameObject _projectile;

    //set random heading, make sure not stick together
    private bool _changeHeading;
    private float _randomHeadingtimer;

    private Vector3 _screenBounds;

    enum Direction
    {
        Idle,
        Left,
        Right
    }
    Direction currentDir = Direction.Idle;

    private bool isAlive = false;



    private void Awake()
    {
        //  boom.SetActive(true);
        //_objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
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
        //_speed = enemyData.Speed;
        _maxSpeed = enemyData.MaxSpeed;
        gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        gameObject.GetComponent<EnemyHealth>().Set();
        _projectile = enemyData.projectile;
        _nextAttackTime = Time.time + 1.0f + Random.Range(-enemyData.AttackDelay * 0.3f, enemyData.AttackDelay * 0.8f);
        if (gameObject.CompareTag("ShieldEnemy"))
        {
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ShieldHealth = _currentShieldHealth;
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ReShield = true;
        }
        isAlive = true;
        _changeHeading = true;
        boom.SetActive(false);
         //boom =_objectPoolManager.GetObjectFromPool("BomberEnemyProjectile");
         //boom.SetActive(false);

         _velocity = Vector3.zero;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        //boom.SetActive(true);
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
        //_acceleration += WanderBehavior.Calculate(gameobject, weight);
        _acceleration = (Vector3)(BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, transform.position + _velocity.normalized, _maxSpeed), enemyData.SeekBehaviorWeight));
        _acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 3.0f), enemyData.WanderBehaviorWeight));
        //_acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WallAvoidance.WallAvoidanceCalculation(transform,_botLeftBound.position.x,_topRightBound.position.x,_topRightBound.position.y,_botLeftBound.position.y),enemyData.WallAvoidWeight));
        if (_changeHeading)
        {
            var xDir = Random.Range(0, 2) == 1 ? -1 : 1;
            var yDir = Random.Range(0, 2) == 1 ? -1 : 1;
            _acceleration.x *= xDir;
            _acceleration.y *= yDir;
            _changeHeading = false;
        }

        if (_velocity.sqrMagnitude > _maxSpeed)
        {
            var speed = _velocity.magnitude;
            _velocity.Normalize();
            _velocity /= speed;
            _velocity *= _maxSpeed;
        }

        _velocity += _acceleration * Time.deltaTime ;

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
        if (movingDir != currentDir)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0.1f * Time.deltaTime);
            currentDir = movingDir;
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(heading.x * -enemyData.Bomber_tiltingAngle + (Time.deltaTime * 2.0f), Vector3.forward);
        }
        if (isAlive)
        {
            if (_randomHeadingtimer < Time.time)
            {
                _randomHeadingtimer = Time.time + enemyData.RandomHeadingTimer;
                _changeHeading = true;
            }
        //Shooting  
            if (_nextAttackTime < Time.time)
            {
                //boom.SetActive(false);
                animator.SetBool("Shoot", true);
                Invoke("unplayAnimation", 0.5f);
                //boom.SetActive(false);
                //var targetlist = LevelManager.Instance.Train.GetTurrets();
                _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.18f, enemyData.AttackDelay * 0.18f);
                Invoke("delayshoot", 0.45f);
            }
            //else
            //{
            //    boom.SetActive(true);
            //
            //}

            //if (_nextAttackTime < (Time.time + enemyData.AttackDelay - enemyData.AttackDelay * 0.1f))
            //    boom.SetActive(false);
            //else
            //    boom.SetActive(true);
        }
    }
    private void RecycleBomberEnemy()
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
            //GameObject projectile = _objectPoolManager.GetObjectFromPool("BomberEnemyProjectile");
            boom.GetComponent<EnemyProjectile>().PlayParticle(transform.position);
            RecycleBomberEnemy();
       }
    }
    private void unplayAnimation()
    {
        animator.SetBool("Shoot", false);
    }

    private void delayshoot()
    {
        var targetlist = _trainData.ListTurret;
        int targetSize = targetlist.Length;
        int randomtarget = Random.Range(0, targetSize);
        //GameObject projectile = _objectPoolManager.GetObjectFromPool("BomberEnemyProjectile");
        //projectile.transform.position = transform.position;
        //Vector3 targetPos = targetlist[randomtarget].gameObject.transform.position;
        //var target = targetlist[randomtarget];
        //if (target)
        //{
        //    Vector3 targetPos = target.gameObject.transform.position;
        //projectile.SetActive(true);
        //projectile.GetComponent<EnemyProjectile>().SetData(targetPos, enemyData.Bomber_AttackSpeed,enemyData.Bomber_AttackDamage,EnemyTypeCheck.Type.Bomber);
        //projectile.GetComponent<EnemyProjectile>().SetData(Vector3.zero, enemyData.Bomber_AttackSpeed,enemyData.Bomber_AttackDamage,EnemyTypeCheck.Type.Bomber);

        //}
        boom = _objectPoolManager.GetObjectFromPool("BomberEnemyProjectile");
        boom.transform.position = transform.position;
        boom.SetActive(true);
        boom.GetComponent<EnemyProjectile>().SetData(Vector3.zero, enemyData.Bomber_AttackSpeed, enemyData.Bomber_AttackDamage, EnemyTypeCheck.Type.Bomber);
    }

}
