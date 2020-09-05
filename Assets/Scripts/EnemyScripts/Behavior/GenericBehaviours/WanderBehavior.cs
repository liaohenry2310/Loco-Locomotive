using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WanderBehavior 
{
    public static Vector2 WanderMove(Transform agent, float mWanderRadius, float mWanderDistance, float mWanderJitter, float speed, Vector2 velocity)
    {

        Vector2 mWanderPoint = Vector2.zero;
        mWanderPoint = mWanderPoint + Random.insideUnitCircle * mWanderJitter;
        mWanderPoint = mWanderPoint.normalized * mWanderRadius;
        //wanderPoint.y += wanderDistance;
        var wanderTarget = mWanderPoint + new Vector2( 0.0f, mWanderDistance );
        var localToWorld = agent.transform.localToWorldMatrix;
        Vector2 destination = localToWorld.MultiplyVector((Vector3)wanderTarget);
        mWanderPoint = SeekBehaviour.SeekMove(agent, destination, speed, velocity);
        //mRenderPoint = destination;
        return mWanderPoint;
    }
}
