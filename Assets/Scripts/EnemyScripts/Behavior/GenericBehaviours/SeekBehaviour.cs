using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehaviour : MonoBehaviour
{
    public Vector2 SeekMove(Transform agent, Vector2 direction, float speed, Vector2 velocity)
    {
        //Steering
        Vector2 posToDest = direction - (Vector2)agent.transform.localPosition;
        Vector2 desiredVelocity = posToDest.normalized * speed;
        return desiredVelocity - velocity;

    }
}