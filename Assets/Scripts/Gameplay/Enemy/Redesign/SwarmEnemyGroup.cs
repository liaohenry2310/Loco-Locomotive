using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyGroup : MonoBehaviour
{
    //data
    [SerializeField] private TrainData _trainData = null;
    public SwarmEnemyData enemyData;
    private float _currentHealth=0.0f;
    private float _nextAttackTime = 0.0f;
    private int _spawnSize = 0;
    private bool isAlive = false;
    //For behavior
    private Vector3 _velocity;
    //For position
    private Transform _topright;
    private Transform _bottomLeft;
    //For SwarmGroup
    private const float agentDensity = 0.08f;
    private List<SwarmEnemy> swarmNeighbors = new List<SwarmEnemy>();
    private List<Transform> swarmNeighborsTrans = new List<Transform>();
    private bool spawnGroup = false;
    private bool spawnSelected = false;
    private Transform _swarmSpawnPos;

    //target
    private GameObject currentTarget;
    private Vector3 targetPos;
    private Vector3 targetDirection;
    //objectpool
    private ObjectPoolManager _objectPoolManager = null;

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }
    private void RecycleSwarmEnemyGroup()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _objectPoolManager.RecycleObject(gameObject);
        Debug.Log("RecycleSwarmEnemyGroup!");
    }


    public void SetNewData(Transform topRight, Transform bottomLeft)
    {
        //Reset all relevant gameplay data so it can be used again when recieved by the object pooler.
        _topright = topRight;
        _bottomLeft = bottomLeft;
        //_currentHealth = enemyData.MaxHealth;
        //gameObject.GetComponent<EnemyHealth>().health = _currentHealth;
        _nextAttackTime = enemyData.AttackDelay;
        isAlive = true;
    }
    public void SetSwarmSpawnPos(Transform spawnPosition)
    {
        _swarmSpawnPos = spawnPosition;
    }

    void Update()
    {
        if (swarmNeighbors.Count != 0&&swarmNeighborsTrans.Count!=0)
        {
           CheckAlive();
           AttackMode();
           GroupBehaviours();
        }
    }
    public void CheckAlive()
    {
        for (int i = swarmNeighbors.Count-1; i>=0; --i)
        {
            var agent = swarmNeighbors[i];
            if (!agent.Alive)
            {
                swarmNeighborsTrans.Remove(agent.transform);
                swarmNeighbors.Remove(agent);
            }
        }
    }
    public void GroupBehaviours()
    {
        Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);
       

        foreach (var agent in swarmNeighbors)
        {

            if (!agent.Attacking)
            {
                 _acceleration = BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(agent.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 1.0f), enemyData.WanderBehaviorWeight);
                 _acceleration += (Vector3)BehaviourUpdate.BehaviourUpdated(CohesionBehavior.CalculateMove(agent.transform, swarmNeighborsTrans),enemyData.CohesionBehaviorWeight);
                 _acceleration += (Vector3)BehaviourUpdate.BehaviourUpdated(SeparationBehavior.SeparationMove(agent.transform, swarmNeighborsTrans, enemyData.SeparationBehaviorRadius),enemyData.SeparationBehaviorWeight);
                 _acceleration += (Vector3)BehaviourUpdate.BehaviourUpdated(AlignmentBehavior.CalculateMove(agent.transform, swarmNeighborsTrans),enemyData.AlignmentBehaviorWeight);
                 agent.Velocity += _acceleration * Time.deltaTime;
                 if (agent.Velocity.magnitude > enemyData.MaxSpeed)
                 {
                     agent.Velocity.Normalize();
                     agent.Velocity *= enemyData.MaxSpeed;
                 }


                // _bottomLeft.position.x, _topright.position.x, _topright.position.y, _bottomLeft.position.y
                if (Mathf.Abs(transform.position.x - _bottomLeft.position.x) < 1.0f)
                {
                    //_velocity = new Vector3(-_velocity.x, _velocity.y, _velocity.z);
                    _velocity.x *= -1;
                }
                if (Mathf.Abs(transform.position.x - _topright.position.x) < 1.0f)
                {
                    //_velocity = new Vector3(-_velocity.x, _velocity.y, _velocity.z);
                    _velocity.x *= -1;
                }
                if (Mathf.Abs(transform.position.y - _topright.position.y) < 1.0f)
                {
                    //_velocity = new Vector3(_velocity.x, -_velocity.y, _velocity.z);
                    _velocity.y *= -1;
                }
                if (Mathf.Abs(transform.position.y - _bottomLeft.position.y) < 1.0f)
                {
                    //_velocity = new Vector3(_velocity.x, -_velocity.y, _velocity.z);
                    _velocity.y *= -1;
                }
                //agent.transform.position += agent.Velocity * Time.deltaTime;
            }
            else
            {
                _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(agent.transform, agent.Target, enemyData.Swarm_AttackSpeed), enemyData.SeekBehaviorWeight);
                agent.Velocity += _acceleration * Time.deltaTime;
            }
            Vector3 deltaPos= agent.Velocity * Time.deltaTime;
            if (deltaPos.x ==0 && deltaPos.y ==0)
            {
                Debug.Log("Enemy is stuck!");
            }

            agent.transform.position += agent.Velocity * Time.deltaTime;
            //_acceleration += (Vector3)(BehaviourUpdate.BehaviourUpdated(WallAvoidance.WallAvoidanceCalculation(agent.transform, _bottomLeft.position.x, _topright.position.x, _topright.position.y, _bottomLeft.position.y), enemyData.WallAvoidWeight));
        }
            //transform.position += _velocity * Time.deltaTime;
        //        {
        //            move = BehaviourUpdate.BehaviourUpdated(CohesionBehavior.CalculateMove(agent.transform, swarmNeighbors), 5.0f);
        //            move *= speed;
        //            if (move.sqrMagnitude > squareMaxSpeed)
        //            {
        //                move = move.normalized * maxSpeed;
        //            }
        //            agent.transform.position += (Vector3)move * Time.deltaTime;
        //        }

    }
    public void AttackMode()
    {

        if (_nextAttackTime < Time.time && swarmNeighbors.Count > 0)
        {

                int num = Random.Range(0, swarmNeighborsTrans.Count);
                var agent = swarmNeighbors[num];
                var agentTrans = swarmNeighborsTrans[num];
                agent.Attacking = true;
                Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);
                var targetlist = _trainData.ListTurret;
                int targetSize = targetlist.Length;
                int randomtarget = Random.Range(0, targetSize - 1);
                _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);
                targetPos = targetlist[randomtarget].gameObject.transform.position;
                agent.Target = targetPos;
           
        }
    }
    public void SpawnGroup(int numToSpawn)
    {
        _spawnSize = numToSpawn;

        for (int j = 0; j < _spawnSize; ++j)
        {
            GameObject _enemyType;
            _enemyType = _objectPoolManager.GetObjectFromPool("SwarmEnemy");
            _enemyType.transform.position = new Vector3(_swarmSpawnPos.position.x+Random.Range(-0.5f, 0.5f), _swarmSpawnPos.position.y+Random.Range(-0.5f, 0.5f), _swarmSpawnPos.localPosition.z);
            _enemyType.SetActive(true);
            var swarmEnemy = _enemyType.gameObject.GetComponent<SwarmEnemy>();
            swarmEnemy.Alive = true;
            swarmEnemy.SetNewData(enemyData);
            swarmNeighbors.Add(_enemyType.GetComponent<SwarmEnemy>());
            swarmNeighborsTrans.Add(_enemyType.transform);
            //enemy.transform.parent = gameObject.transform;
            //enemy.transform.localPosition = Vector2.zero + Random.insideUnitCircle * countGroupSize * agentDensity;
        }
      //squareMaxSpeed = maxSpeed * maxSpeed;
    }

}
