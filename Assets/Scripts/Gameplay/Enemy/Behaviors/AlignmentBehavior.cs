using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlignmentBehavior 
{
    public static Vector2 CalculateMove(Transform agent, List<Transform> neighbors)
    {
        if (neighbors.Count == 0)
        {
            return Vector2.zero;
        }
        Vector2 alignmentMove = Vector2.zero;
        foreach (Transform item in neighbors)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        
        alignmentMove /= neighbors.Count;

        return alignmentMove;
    }

}
