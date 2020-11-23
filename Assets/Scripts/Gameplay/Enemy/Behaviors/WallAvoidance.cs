using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallAvoidance
{
    public static Vector2 WallAvoidanceCalculation (Transform agent,float minX,float maxX,float minY,float maxY)
    {
        Vector2 avoidMove = new Vector2(0.0f, 0.0f);
        avoidMove += (Vector2)agent.transform.position;
        if (agent.transform.position.x < minX)
        {
            avoidMove.x *= -1;
        }
        if (agent.transform.position.x > maxX)
        {
            avoidMove.x *= -1;
        }
        if (agent.transform.position.y < minY)
        {
            avoidMove.y *= -1;
        }
        if (agent.transform.position.y > maxY)
        {
            avoidMove.y *= -1;
        }

        return avoidMove;
    }
}
