using System.Collections.Generic;
using UnityEngine;

public class ArriveBehaviour : MonoBehaviour
{



    // Start is called before the first frame update
    //void Start()
    //{
    //    //gameObject.GetComponentsInChildren<Transform>() ->
    //    //foreach (var item in basicEnemypaths.gameObject.GetComponentsInChildren<Transform>())
    //    //{
    //    //    paths.Add(item);
    //    //}
    //    for (int i = 0; i < basicEnemypaths.transform.childCount; ++i)
    //    {
    //        paths.Add(basicEnemypaths.transform.GetChild(i));
    //
    //    }
    //    TowardNextPoint();
    //
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //
    //    Vector2 force = Behavior();
    //    Vector2 acceleration = force / 0.5f;
    //    velocity += acceleration * Time.deltaTime;
    //    speed = velocity.magnitude;
    //
    //    if (speed > maxSpeed)
    //    {
    //        velocity = velocity / (speed * maxSpeed);
    //    }
    //    if (speed > 0.0f)
    //    {
    //        direction = velocity.normalized;
    //    }
    //
    //    transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * Time.deltaTime;
    //
    //
    //
    //    if (Vector2.Distance(gameObject.transform.position, paths[nextPoint].gameObject.transform.position) < 1.0f)
    //    {
    //        delay -= Time.deltaTime;
    //        if (delay <= 0.0f)
    //        {
    //            TowardNextPoint();
    //            delay = stopDelay;
    //        }
    //    }
    //    else
    //    {
    //        currentPos = gameObject.transform.position;
    //        direction = nextPos - currentPos;
    //        direction.Normalize();
    //        //transform.Translate(pos , Space.World);
    //
    //    }
    //}

    public Vector2 TowardNextPoint(List<Transform> paths)
    {
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

    public Vector2 ArriveMove(Transform agent,Vector2 direction,float maxSpeed,Vector2 velocity)
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
