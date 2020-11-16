using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallAvoidance
{
    public static Vector2 WallAvoidanceCalculation (Transform agent,float minX,float maxX,float minY,float maxY)
    {
        Vector2 avoidMove = new Vector2(0.0f, 0.0f);
        float distance = 1.0f;
        if (Mathf.Abs(agent.position.x-minX)<distance)
        {
            avoidMove.x = 1.0f;
        }
        if (Mathf.Abs(agent.position.x - maxX) < distance)
        {
            avoidMove.x = -1.0f;
        }
        if (Mathf.Abs(agent.position.y - minY) < distance)
        {
            avoidMove.x = 1.0f;
        }
        if (Mathf.Abs(agent.position.y - maxY) < distance)
        {
            avoidMove.x = -1.0f;
        }

        return avoidMove;
    }
}
