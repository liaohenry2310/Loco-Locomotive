using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyGroup : MonoBehaviour
{
    bool spawnGroup = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnGroup = true;
    }

    // Update is called once per frame
    void Update()
    {


        if (spawnGroup)
        {
            SwarmGroup();
        }
    }

    void SwarmGroup()
    {
        GameObject enemy = null;
        List<GameObject> swarmNeighbors = new List<GameObject>();

        SwarmEnemy swarmEnemy = enemy.GetComponent<SwarmEnemy>();
        swarmNeighbors.Add(enemy);
        for (int j = 0; j < swarmEnemy.countGroupSize; ++j)
        {
            swarmNeighbors.Add(Instantiate(enemy));
        }
        for (int k = 0; k < swarmNeighbors.Count; ++k)
        {
            //swarmNeighbors[k].GetComponent<SwarmEnemy>().swarmNeighbors = swarmNeighbors;

        }
        spawnGroup = false;
    }

   // Vector2 SeekBehavior()
   // {
   //     //Steering
   //     //Vector2 posToDest = new Vector2(paths[nextPoint].transform.position.x, paths[nextPoint].transform.position.y) - new Vector2(transform.position.x,transform.position.y);
   //     //Vector2 desiredVelocity = posToDest.normalized * speed;
   //     //return desiredVelocity - velocity;
   //
   //     //Arrive
   //     //Vector2 posToDest = (TowardNextPos()) - new Vector2(transform.position.x, transform.position.y);
   //     //float distance = posToDest.magnitude;
   //     //if (distance <= 0.0f)
   //     //    return new Vector2();
   //     //
   //     //float speed = Mathf.Min(maxSpeed, distance);
   //     //
   //     //Vector2 desiredVelocity = posToDest / distance * speed;
   //     //
   //     //return desiredVelocity - velocity;
   // }

}
