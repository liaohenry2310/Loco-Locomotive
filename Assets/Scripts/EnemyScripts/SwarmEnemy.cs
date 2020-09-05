using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemy : MonoBehaviour
{
    // path
    List<Transform> paths = new List<Transform>();
    public GameObject basicEnemypaths;
    //Behaviours
    ArriveBehaviour arrive;
    CohesionBehavior cohesion = new CohesionBehavior();
    SeekBehaviour seek = new SeekBehaviour();
    SeparationBehavior separation = new SeparationBehavior();
    BehaviourUpdate behaviourUpdate = new BehaviourUpdate();
    WanderBehavior wander = new WanderBehavior();
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
    bool isDelay;
    // Change Enemy State .
    private State mCurrentState = State.GroupSeparation;
    enum State
    {
        Nothing,
        GroupCohesion,
        GroupSeparation,
        Attack
    }
    void Start()
    {
        //for (int i = 0; i < GroupOfSize; ++i)
        //{
        //    swarmNeighbors.Add(Instantiate(swarmType));
        //}
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
        mCurrentState = State.GroupSeparation;
    }

    void Update()
    {
        Vector2 move = Vector2.zero;
        float defaultDelay = 1f;
        if (mCurrentState == State.GroupCohesion)
        {
            foreach (var agent in swarmNeighbors)
            {
                move = behaviourUpdate.BehaviourUpdated(cohesion.CalculateMove(agent.transform, swarmNeighbors), 5.0f);
                move *= speed;
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                agent.transform.position += (Vector3)move * Time.deltaTime;

                //move += cohesion.CalculateMove(agent.transform, swarmNeighbors);
                //Vector2 acceleration = move / 0.5f;
                //velocity += acceleration * Time.deltaTime;
                //speed = velocity.sqrMagnitude;
                //
                //if (speed > maxSpeed)
                //{
                //    velocity = velocity / (speed * maxSpeed);
                //}
                //if (speed > 0.0f)
                //{
                //    direction = velocity.normalized;
                //}
                //
                //agent.transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * Time.deltaTime;
            }
            //isDelay = true;
            StartCoroutine(timeDelayFuc(defaultDelay));
            mCurrentState = State.GroupSeparation;

        }
        if (mCurrentState == State.GroupSeparation)
        {
            foreach (var agent in swarmNeighbors)
            {
                move = behaviourUpdate.BehaviourUpdated(separation.SeparationMove(agent.transform, swarmNeighbors, separationRadius), 5.0f);
                //move=behaviourUpdate.BehaviourUpdated(wander.WanderMove(agent.transform,5.0f,2.0f,10.0f,speed,velocity), 5.0f);
                move *= speed;
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                agent.transform.position += (Vector3)move * Time.deltaTime;
            }
            //isDelay = true;
            StartCoroutine(timeDelayFuc(defaultDelay));
            mCurrentState = State.GroupCohesion;
        }
        //foreach (var agent in swarmNeighbors)
        //{
        //    move += behaviourUpdate.BehaviourUpdated(seek.SeekMove()
        //         move *= speed;
        //    if (move.sqrMagnitude > squareMaxSpeed)
        //    {
        //        move = move.normalized * maxSpeed;
        //    }
        //    agent.transform.position += (Vector3)velocity * Time.deltaTime;
        //}
        //Vector2 Seekforce = seek.SeekMove(this.transform,direction,speed,velocity);
        //Vector2 Seperationforce = separation.SeparationMove(this.transform, swarmNeighbors, separationRadius);
        //Vector2 force = Seekforce + Seperationforce;
        //
        //Vector2 acceleration = force / 0.5f;
        //velocity += acceleration * Time.deltaTime;
        //speed = velocity.magnitude;
        //
        //if (speed > maxSpeed)
        //{
        //    velocity = velocity / (speed * maxSpeed);
        //}
        //if (speed > 0.0f)
        //{
        //    direction = velocity.normalized;
        //}
        //
        //transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * Time.deltaTime;
        //if (Vector2.Distance(gameObject.transform.position, nextPos) < 1.0f)
        //{
        //    delay -= Time.deltaTime;
        //    if (delay <= 0.0f)
        //    {
        //        ArriveBehaviour arrive =new ArriveBehaviour();
        //        nextPos=arrive.TowardNextPoint(paths);
        //        delay = stopDelay;
        //    }
        //}
        //else
        //{
        //    currentPos = gameObject.transform.position;
        //    direction = nextPos - currentPos;
        //    direction.Normalize();
        //    //transform.Translate(pos , Space.World);
        //}

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

    public IEnumerator timeDelayFuc(float timedelay)
    {
        yield return new WaitForSeconds(timedelay);
        //isDelay = false ;
    }


}
