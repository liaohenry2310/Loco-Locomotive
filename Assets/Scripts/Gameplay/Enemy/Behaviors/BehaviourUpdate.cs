using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourUpdate
{
    public static Vector2 BehaviourUpdated(Vector2 behaviourMove, float weight)
    {
        Vector2 move = Vector2.zero;
        Vector2 partialMove = behaviourMove;
        if (partialMove != Vector2.zero)
        {
            if (partialMove.sqrMagnitude > weight )
            {
                partialMove.Normalize();
                partialMove *= weight;
            }
        }
            move += partialMove;
        return move;
    }
}
