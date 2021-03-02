using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WanderBehavior 
{
    public static Vector2 WanderMove(Transform agent, float mWanderRadius, float mWanderDistance, float mWanderJitter, float speed)
    {
        //var rx = Random.Range( -20.0f,20.0f);
        //var ry = Random.Range( -20.0f,20.0f);
        //Vector2 mWanderPoint = new Vector2(rx,ry);
        //mWanderPoint = mWanderPoint + Random.insideUnitCircle * mWanderJitter;
        Vector2 mWanderPoint = Vector2.zero;
        mWanderPoint.x += Random.Range(-mWanderJitter, mWanderJitter);
        mWanderPoint.y += Random.Range(-mWanderJitter, mWanderJitter);
        mWanderPoint = mWanderPoint.normalized;
        mWanderPoint *= mWanderRadius;
        //wanderPoint.y += wanderDistance;
        var wanderTarget = mWanderPoint + new Vector2( 0.0f, mWanderDistance );
        var localToWorld = agent.transform.localToWorldMatrix;
        Vector2 destination = localToWorld.MultiplyVector((Vector3)wanderTarget);
        mWanderPoint = SeekBehaviour.SeekMove(agent, destination, speed);
        //mRenderPoint = destination;
        return mWanderPoint;
    }
}
