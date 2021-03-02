using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyGroup : MonoBehaviour
{
    //data
    [SerializeField] private TrainData _trainData = null;
    public SwarmEnemyData enemyData;
    private float _nextAttackTime = 0.0f;
    private int _spawnSize = 0;
    //For behavior
    private Vector3 _velocity;
    //For position
    private Transform _topright;
    private Transform _bottomLeft;
    //For SwarmGroup
    private const float agentDensity = 0.08f;
    private List<SwarmEnemy> swarmNeighbors = new List<SwarmEnemy>();
    private List<Transform> swarmNeighborsTrans = new List<Transform>();
    private Transform _swarmSpawnPos;
    private SpriteRenderer _sprite;
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
        _nextAttackTime = enemyData.AttackDelay+Time.time;
        
        //isAlive = true;
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
           GroupBehaviours();
           AttackMode();
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
                if (swarmNeighbors.Count >1)
                {
                    _acceleration = (Vector3)(BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, transform.position + _velocity.normalized, enemyData.Speed), enemyData.SeekBehaviorWeight));
                    _acceleration += (Vector3)BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(agent.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 1.0f), enemyData.WanderBehaviorWeight);
                    _acceleration += (Vector3)BehaviourUpdate.BehaviourUpdated(CohesionBehavior.CalculateMove(agent.transform, swarmNeighborsTrans),enemyData.CohesionBehaviorWeight);
                }
                else
                {
                    _acceleration = (Vector3)(BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(transform, transform.position + _velocity.normalized, enemyData.Speed), enemyData.SeekBehaviorWeight));
                    _acceleration += (Vector3)BehaviourUpdate.BehaviourUpdated(WanderBehavior.WanderMove(agent.transform, enemyData.WanderRadius, enemyData.WanderDistance, enemyData.WanderJitter, 1.0f), enemyData.WanderBehaviorWeight);

                }
                agent.Velocity += _acceleration  * Time.deltaTime;


                if (agent.transform.position.x < _bottomLeft.position.x)
                {
                    agent.Velocity *= -1;
                }
                if (agent.transform.position.x > _topright.position.x)
                {
                    agent.Velocity *= -1;
                }
                if (agent.transform.position.y < _topright.position.y)
                {
                    agent.Velocity *= -1;
                }
                if (agent.transform.position.y > _bottomLeft.position.y)
                {
                    agent.Velocity *= -1;
                }
                //agent.transform.position += agent.Velocity * Time.deltaTime;
                if (agent.Velocity.sqrMagnitude > enemyData.MaxSpeed)
                {
                    var speed = agent.Velocity.magnitude;
                    agent.Velocity.Normalize();
                    agent.Velocity /= speed;
                    agent.Velocity *= enemyData.Speed;
                }
            }
            else
            {
                //agent.Velocity = Vector3.zero;
                agent.Shake();
                //agent.SetTarget(agent.Target);
                _acceleration = BehaviourUpdate.BehaviourUpdated(SeekBehaviour.SeekMove(agent.transform, agent.Target, enemyData.Swarm_AttackSpeed), enemyData.SeekBehaviorWeight);
                if (agent.transform.position.x < _bottomLeft.position.x)
                {
                    agent.Velocity *= -1;
                }
                if (agent.transform.position.x > _topright.position.x)
                {
                    agent.Velocity *= -1;
                }

                agent.Velocity += _acceleration * Time.deltaTime;
            }

            agent.transform.position += agent.Velocity* Time.deltaTime*(enemyData.Speed);
            var heading = agent.Velocity.normalized;
            agent.transform.rotation = Quaternion.AngleAxis(heading.x * -enemyData.Swarm_tiltingAngle + (Time.deltaTime * 2.0f), Vector3.forward);

               }


    }
    public void AttackMode()
    {

        if (_nextAttackTime < Time.time && swarmNeighbors.Count >= 0)
        {

                int num = Random.Range(0, swarmNeighborsTrans.Count);
                var agent = swarmNeighbors[num];
                var agentTrans = swarmNeighborsTrans[num];
                agent.Attacking = true;
                Vector3 _acceleration = new Vector3(0.0f, 0.0f, 0.0f);
                var targetlist = _trainData.ListTurret;
                int targetSize = targetlist.Length;
                int randomtarget = Random.Range(0, targetSize);
                _nextAttackTime = Time.time + enemyData.AttackDelay + Random.Range(-enemyData.AttackDelay * 0.1f, enemyData.AttackDelay * 0.1f);
                targetPos = targetlist[randomtarget].gameObject.transform.position;
                agent.Target = targetPos;
           
        }
    }
    public void SpawnGroup(int numToSpawn,bool shield)
    {
        _spawnSize = numToSpawn;
        if (!shield)
        {
            for (int j = 0; j < _spawnSize; ++j)
            {
                GameObject _enemyType;
                _enemyType = _objectPoolManager.GetObjectFromPool("SwarmEnemy");
                _enemyType.transform.position = new Vector3(_swarmSpawnPos.position.x+Random.Range(-0.5f, 0.5f), _swarmSpawnPos.position.y+Random.Range(-0.5f, 0.5f), _swarmSpawnPos.localPosition.z);
                _enemyType.SetActive(true);
                var swarmEnemy = _enemyType.gameObject.GetComponent<SwarmEnemy>();
                swarmEnemy.Velocity = Vector3.zero;
                swarmEnemy.Attacking = false;
                swarmEnemy.Alive = true;
                swarmEnemy.SetNewData(enemyData);
                swarmNeighbors.Add(_enemyType.GetComponent<SwarmEnemy>());
                swarmNeighborsTrans.Add(_enemyType.transform);
                //enemy.transform.parent = gameObject.transform;
                //enemy.transform.localPosition = Vector2.zero + Random.insideUnitCircle * countGroupSize * agentDensity;
            }
        }
        if (shield)
        {
            for (int j = 0; j < _spawnSize; ++j)
            {
                GameObject _enemyType;
                _enemyType = _objectPoolManager.GetObjectFromPool("Swarm_Shield");
                _enemyType.transform.position = new Vector3(_swarmSpawnPos.position.x + Random.Range(-0.5f, 0.5f), _swarmSpawnPos.position.y + Random.Range(-0.5f, 0.5f), _swarmSpawnPos.localPosition.z);
                _enemyType.SetActive(true);
                var swarmEnemy = _enemyType.gameObject.GetComponent<SwarmEnemy>();
                swarmEnemy.Attacking = false;
                swarmEnemy.Alive = true;
                swarmEnemy.SetNewData(enemyData);
                swarmNeighbors.Add(_enemyType.GetComponent<SwarmEnemy>());
                swarmNeighborsTrans.Add(_enemyType.transform);
            }
        }
      //squareMaxSpeed = maxSpeed * maxSpeed;
    }

}
