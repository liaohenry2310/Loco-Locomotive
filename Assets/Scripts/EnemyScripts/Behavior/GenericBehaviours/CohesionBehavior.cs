using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CohesionBehavior : MonoBehaviour
{
    public Vector2 CalculateMove (Transform agent, List<Transform> neighbors)
    {
        //if no neighbors, return no adjustment
        if (neighbors.Count == 0)
            return Vector2.zero;
        //add all points together and average
        Vector2 cohesionMove = Vector2.zero;
        foreach (Transform item in neighbors)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= neighbors.Count;
        //create offset from agent position
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }
}
