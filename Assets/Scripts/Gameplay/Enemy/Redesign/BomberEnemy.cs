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
    private float _currentShieldHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;
    private GameObject _projectile;




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
        _nextAttackTime = enemyData.AttackDelay;
        if (gameObject.CompareTag("ShieldEnemy"))
        {
            gameObject.GetComponentInChildren<EnemyShieldHealth>().ShieldHealth = _currentShieldHealth;
        }
        isAlive = true;
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
        //_acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WallAvoidance.WallAvoidanceCalculation(transform,_botLeftBound.position.x,_topRightBound.position.x,_topRightBound.position.y,_botLeftBound.position.y),enemyData.WallAvoidWeight));
        _velocity += _acceleration * Time.deltaTime;

        if (  _velocity.magnitude > enemyData.MaxSpeed)
        {
            _velocity.Normalize();
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

        transform.position += _velocity * Time.deltaTime;


        //Shooting
        if (_nextAttackTime < Time.time)
        {
            //var targetlist = LevelManager.Instance.Train.GetTurrets();
            var targetlist = _trainData.ListTurret;
            int targetSize = targetlist.Length;
            int randomtarget = Random.Range(0, targetSize-1);
            _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);

            GameObject projectile = _objectPoolManager.GetObjectFromPool("BomberEnemyProjectile");
            projectile.transform.position = transform.position;
            Vector3 targetPos = targetlist[randomtarget].gameObject.transform.position;
            projectile.SetActive(true);
            projectile.GetComponent<EnemyProjectile>().SetData(targetPos, EnemyTypeCheck.Type.Bomber);
            

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
