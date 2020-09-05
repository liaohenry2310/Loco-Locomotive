using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehavior : MonoBehaviour
{
    public Vector2 SeparationMove(Transform agent, List<Transform> neighbors,float separationRadius)
    {
        //if no neighbors, return no adjustment
        if (neighbors.Count == 0)
            return Vector2.zero;
        //add all points together and average
        Vector2 separationMove = Vector2.zero;
        int nAvoid = 0;
        foreach (Transform item in neighbors)
        {

            if (Vector2.SqrMagnitude(item.position-agent.transform.position ) < separationRadius * separationRadius&& item.gameObject!=agent .gameObject)
            {
                nAvoid++;
                separationMove += (Vector2)(agent.transform.position - item.transform.position);
            }
        }
        if (nAvoid > 0)
            separationMove /= nAvoid;
        return separationMove;
    }
}
