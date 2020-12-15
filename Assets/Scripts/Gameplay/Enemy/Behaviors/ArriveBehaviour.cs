using System.Collections.Generic;
using UnityEngine;

public static class ArriveBehaviour 
{

    public static Vector2 TowardNextPoint(GameObject pathObject)
    {
        List<Transform> paths = new List<Transform>();
        for (int i = 0; i < pathObject.transform.childCount; ++i)
        {
            paths.Add(pathObject.transform.GetChild(i));
        }
        Vector2 nextPos;
        int nextPoint=0;
        int randomNum = Random.Range(0, paths.Count - 1);
        while (randomNum == nextPoint)
        {
            randomNum = Random.Range(0, paths.Count - 1);
        }
        nextPoint = randomNum;
        nextPos = paths[nextPoint].gameObject.transform.position;
        return nextPos;
    }

    public static Vector2 ArriveMove(Transform agent,Vector2 direction,float maxSpeed,Vector2 velocity)
    {
        //Steering
        //Vector2 posToDest = new Vector2(paths[nextPoint].transform.position.x, paths[nextPoint].transform.position.y) - new Vector2(transform.position.x,transform.position.y);
        //Vector2 desiredVelocity = posToDest.normalized * speed;
        //return desiredVelocity - velocity;

        //Arrive
        Vector2 posToDest = direction - (Vector2)agent.transform.position;
        float distance = posToDest.magnitude;
        if (distance <= 0.0f)
            return new Vector2();
        
        float speed = Mathf.Min(maxSpeed, distance);
        
        Vector2 desiredVelocity = posToDest / distance * speed;
        
        return desiredVelocity - velocity;
    }
}
