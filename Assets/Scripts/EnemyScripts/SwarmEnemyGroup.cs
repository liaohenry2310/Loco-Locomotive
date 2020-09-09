using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyGroup : Enemy
{
    // path
    public List<Transform> paths = new List<Transform>();
    public GameObject basicEnemypaths;
    //Behaviours
     
    //separation
    public float separationRadius;
    //For behavior
    public float maxSpeed;
    float squareMaxSpeed;
    public float speed;
    Vector2 direction;
    Vector2 currentPos;
    Vector2 velocity;
    Vector2 nextPos;
    int nextPoint;
    float delay;
    public float stopDelay;
    //For position
    public Transform topL, bomR;
    //For SwarmGroup
    const float agentDensity = 0.08f;
    public int countGroupSize = 0;
    public GameObject swarmType;
    List<Transform> swarmNeighbors = new List<Transform>();
    bool spawnGroup = false;
    Vector2 moveToPos = Vector2.zero;

    //Time delay
    bool isSwitchingBehaviour = false;
     
    //target
    private GameObject currentTarget;
    private Vector3 targetPos;
    private Vector3 targetDirection;


    // Change Enemy State .
    public enum State
    {
        Nothing,
        GroupCohesion,
        GroupSeparation,
        GroupMove,
        Attack
    }
    private State mCurrentState = State.GroupSeparation;

    void Start()
    {
        delay = stopDelay;
        speed = 2.0f;

        //SpawnGroup();
        TowardNextPoint();
    }

    void Update()
    {

        Vector2 move = Vector2.zero;
        float defaultDelay = 1f;
        if (Vector2.Distance(gameObject.transform.position, paths[nextPoint].gameObject.transform.position) < 1.0f)
        {
            delay -= Time.deltaTime;
            if (delay <= 0.0f)
            {
                TowardNextPoint();
                delay = stopDelay;
            }
        }
        else
        {
            currentPos = gameObject.transform.position;
            direction = nextPos - currentPos;
            direction.Normalize();
        }

        Debug.Log($"Current State: {mCurrentState}");
        switch (mCurrentState)
        {
            case State.Attack:
                if (swarmNeighbors.Count!=0)
                {
                    
                    targetPos = GetTargetPositison().position;
                    GameObject currentEnemy;
                    Vector3 aimmingPos;
                    currentEnemy = swarmNeighbors[0].gameObject;
                    aimmingPos = currentEnemy.GetComponent<SwarmEnemy>().UpdatePos(targetPos);

                    currentEnemy.transform.position += aimmingPos * Time.deltaTime;
                }
                if (!isSwitchingBehaviour)
                {
                  StartCoroutine(TimeDelayFuc(defaultDelay, State.GroupCohesion));
                  isSwitchingBehaviour = true;
                }
                break;
            case State.GroupCohesion:
                foreach (var agent in swarmNeighbors)
                {
                    move = BehaviourUpdate.BehaviourUpdated(CohesionBehavior.CalculateMove(agent.transform, swarmNeighbors), 5.0f);
                    move *= speed;
                    if (move.sqrMagnitude > squareMaxSpeed)
                    {
                        move = move.normalized * maxSpeed;
                    }
                    agent.transform.position += (Vector3)move * Time.deltaTime;
                }

                if (!isSwitchingBehaviour)
                {
                    StartCoroutine(TimeDelayFuc(defaultDelay, State.GroupMove));
                    isSwitchingBehaviour = true;
                }
                break;
            case State.GroupSeparation:
                foreach (var agent in swarmNeighbors)
                {
                    move = BehaviourUpdate.BehaviourUpdated(SeparationBehavior.SeparationMove(agent.transform, swarmNeighbors, separationRadius), 2.0f);
                    //move=behaviourUpdate.BehaviourUpdated(wander.WanderMove(agent.transform,5.0f,2.0f,10.0f,speed,velocity), 5.0f);
                    move = (move.normalized * (Time.deltaTime * speed));
                    //if (move.sqrMagnitude > squareMaxSpeed)
                    //{
                    //    move = move.normalized * maxSpeed;
                    //}
                    agent.transform.position += (Vector3)move;
                }

                if (!isSwitchingBehaviour)
                {
                    StartCoroutine(TimeDelayFuc(defaultDelay, State.Attack));
                    isSwitchingBehaviour = true;
                }
                break;
            case State.GroupMove:
                foreach (var agent in swarmNeighbors)
                {
                    agent.transform.position += agent.GetComponent<SwarmEnemy>().UpdatePos(transform.position)*Time.deltaTime;
                }
                //move = BehaviourUpdate.BehaviourUpdated(ArriveBehaviour.ArriveMove(transform, nextPos, maxSpeed, velocity), 2.0f);
                //move = (move.normalized * (Time.deltaTime * speed));

                transform.position += (Vector3)SeekBehaviour.SeekMove(transform, nextPos, speed)*Time.deltaTime; 
                if (!isSwitchingBehaviour)
                {
                    StartCoroutine(TimeDelayFuc(defaultDelay, State.GroupSeparation));
                    isSwitchingBehaviour = true;
                }
                break;
            default:
                Debug.LogWarning($"Case {mCurrentState} Not handled");
                break;
        }





        //transform.position += (Vector3)moveToPos * Time.deltaTime;
    }

    public void SpawnGroup(Vector3 spawnPos)
    {
        GameObject enemy = null;
        for (int j = 0; j < countGroupSize; ++j)
        {
            enemy = Instantiate(swarmType, spawnPos * Random.insideUnitCircle * countGroupSize * agentDensity,   //size of circle base on size of agents , so not gonna be so far apart that they are all flying away from each other.

                Quaternion.identity, transform//Euler(Vector3.forward * Random.Range(0f, 360f)),

                );
            swarmNeighbors.Add(enemy.transform);
            //enemy.transform.parent = gameObject.transform;
        }
        squareMaxSpeed = maxSpeed * maxSpeed;
        mCurrentState = State.GroupCohesion;
        for (int i = 0; i < basicEnemypaths.transform.childCount; ++i)
        {
            paths.Add(basicEnemypaths.transform.GetChild(i));

        }


    }

    public IEnumerator TimeDelayFuc(float timedelay, State newState)
    {
        yield return new WaitForSeconds(timedelay);
        mCurrentState = newState;
        isSwitchingBehaviour = false;
    }
    void TowardNextPoint()
    {
        int randomNum = Random.Range(0, paths.Count - 1);
        while (randomNum == nextPoint)
        {
            randomNum = Random.Range(0, paths.Count - 1);
        }

        nextPoint = randomNum;
        nextPos = paths[nextPoint].gameObject.transform.position;

    }
    Transform GetTargetPositison()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (var potentialTarget in targetList)
        {
            if (potentialTarget != null)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
        }
        return bestTarget;
    }

    
}
