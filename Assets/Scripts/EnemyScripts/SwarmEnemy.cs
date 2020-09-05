using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemy : MonoBehaviour
{
    // path
    List<Transform> paths = new List<Transform>();
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
    Vector2 nextPos = new Vector2(0.0f, 0.0f);
    float delay;
    public float stopDelay;
    //For position
    public Transform topL, bomR;
    //For SwarmGroup
    const float agentDensity = 0.08f;
    public int countGroupSize = 0;
    public GameObject swarmType;
    public List<Transform> swarmNeighbors = new List<Transform>();
    bool spawnGroup = false;
    
    //Time delay
    bool isSwitchingBehaviour = false;

    // Change Enemy State .
    public enum State
    {
        Nothing,
        GroupCohesion,
        GroupSeparation,
        Attack
    }
    private State mCurrentState = State.GroupSeparation;
    void Start()
    {
        delay = stopDelay;
        speed = 2.0f;
        spawnGroup = true;

        GameObject enemy = null;
        for (int j = 0; j < countGroupSize; ++j)
        {
            enemy = Instantiate(swarmType, Random.insideUnitCircle * countGroupSize * agentDensity,   //size of circle base on size of agents , so not gonna be so far apart that they are all flying away from each other.

                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), // random rotation
                transform //this parent transform;
                );
            swarmNeighbors.Add(enemy.transform);
        }
        squareMaxSpeed = maxSpeed * maxSpeed;
        mCurrentState = State.GroupCohesion;
    }

    void Update()
    {
        Vector2 move = Vector2.zero;
        float defaultDelay = 1f;

        Debug.Log($"Current State: {mCurrentState}");
        switch (mCurrentState)
        {
            case State.Attack:
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
                    StartCoroutine(TimeDelayFuc(defaultDelay, State.GroupSeparation));
                    isSwitchingBehaviour = true;
                }
                break;
            case State.GroupSeparation:
                foreach (var agent in swarmNeighbors)
                {
                    move = BehaviourUpdate.BehaviourUpdated(SeparationBehavior.SeparationMove(agent.transform, swarmNeighbors, separationRadius), 5.0f);
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
                    StartCoroutine(TimeDelayFuc(defaultDelay, State.GroupCohesion));
                    isSwitchingBehaviour = true;
                }
                break;
            case State.Nothing:
                break;
            default:
                Debug.LogWarning($"Case {mCurrentState} Not handled");
                break;
        }
    }

    void SpawnGroup()
    {
        if (spawnGroup)
        {
            SpawnGroup();

        }
        //swarmNeighbors.Add(swarmType);
        for (int j = 0; j < countGroupSize; ++j)
        {
            //swarmNeighbors.Add(Instantiate(swarmType));
        }
        spawnGroup = false;
    }

    public IEnumerator TimeDelayFuc(float timedelay, State newState)
    {
        yield return new WaitForSeconds(timedelay);
        mCurrentState = newState;
        isSwitchingBehaviour = false;
    }
}
