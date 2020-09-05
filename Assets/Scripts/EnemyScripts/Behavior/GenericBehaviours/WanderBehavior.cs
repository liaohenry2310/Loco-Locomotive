using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehavior : MonoBehaviour
{
    public SeekBehaviour seek = new SeekBehaviour();
    //public float mWanderRadius = 0f;
    //public float mWanderDistance = 0f;
    //public float mWanderJitter = 0f;
    //Vector2 mWanderPoint = Vector2.zero;
    //Vector2 mRenderPoint = Vector2.zero;
    //Vector2 direction = Vector2.zero;
    //public float speed;
    //public float maxSpeed;
    //Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 force = WanderMove();
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


    }

    public Vector2 WanderMove(Transform agent, float mWanderRadius, float mWanderDistance, float mWanderJitter, float speed, Vector2 velocity)
    {

        Vector2 mWanderPoint = Vector2.zero;
        mWanderPoint = mWanderPoint + Random.insideUnitCircle * mWanderJitter;
        mWanderPoint = mWanderPoint.normalized * mWanderRadius;
        //wanderPoint.y += wanderDistance;
        var wanderTarget = mWanderPoint + new Vector2( 0.0f, mWanderDistance );
        var localToWorld = agent.transform.localToWorldMatrix;
        Vector2 destination = localToWorld.MultiplyVector((Vector3)wanderTarget);
        mWanderPoint = seek.SeekMove(agent, destination, speed, velocity);
        //mRenderPoint = destination;
        return mWanderPoint;
    }
}
