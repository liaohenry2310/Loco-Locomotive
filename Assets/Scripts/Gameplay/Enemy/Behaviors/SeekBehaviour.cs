using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SeekBehaviour 
{
    public static Vector2 SeekMove(Transform agent, Vector2 direction, float speed)
    {
        //Seek
        Vector2 posToDest = direction - (Vector2)agent.transform.position;
        Vector2 desiredVelocity = posToDest.normalized * speed;
        return desiredVelocity ;

    }
}