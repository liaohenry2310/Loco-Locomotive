using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    //call target dir from list.

    public BasicEnemyData enemyData;


    private Vector3 _velocity;
    private float _nextAttackTime = 0.0f;

    private Transform _topRightBound;
    private Transform _botLeftBound;
    private List<Vector2> _targetPositions;
    private EnemySpawner _spawnerbound;
    private float _currentHealth = 0.0f;
    private ObjectPoolManager _objectPoolManager = null;



    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }
    public void SetNewData()// List<Vector2> targetPositions)
    {
        //Reset all relevant gameplay data so it can be used again when recieved by the object pooler.

        _topRightBound = _spawnerbound.TopRight;
        _botLeftBound = _spawnerbound.BottomLeft;
        //_targetPositions = targetPositions;
        _currentHealth = enemyData.MaxHealth;


    }

    void Update()
    {

        FlyAndShootUpdate();
  

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
        _acceleration = BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(this.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 1.0f),enemyData.WanderBehaviorWeight);
        //_acceleration += WallAvoidance.Calculate(gameobject, weight);
        _acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WallAvoidance.WallAvoidanceCalculation(this.transform, _botLeftBound.localPosition.x, _topRightBound.localPosition.x, _topRightBound.localPosition.y,_botLeftBound.localPosition.y),enemyData.WallAvoidWeight));
        _velocity += _acceleration * Time.deltaTime;

        if (  _velocity.magnitude > enemyData.MaxSpeed)
        {
            _velocity.Normalize();
            _velocity *= enemyData.MaxSpeed;
        }

        transform.position += _velocity * Time.deltaTime;

        //Shooting
        //if (_nextAttackTime < Time.time)
        //{
        //    _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);
        //
        //    int targetIndex = Random.Range(0, _targetPositions.Count - 1);
        //    Vector3 targetPos = _targetPositions[targetIndex];
        //    Vector3 bulletDir = targetPos - transform.position;
        //    bulletDir.Normalize();
        //
        //    bulletDir = Quaternion.Euler(0.0f, 0.0f, Random.Range(-15.0f, 15.0f)) * bulletDir; //Randomize the direction of the bullet a small bit.
        //
        //    //Get the bullet from the object pool.
        //    // Set the bullet's direction to bulletDir;
        //    // Set the bullet's position to transform.position;
        //    // Enable the bullet.
        //}
    }
    private void RecycleBasicEnemy()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _objectPoolManager.RecycleObject(gameObject);
    }
}
