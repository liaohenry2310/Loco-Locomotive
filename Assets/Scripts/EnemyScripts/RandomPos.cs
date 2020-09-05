using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    public Transform topL, bomR;
    float x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        Direction();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2)transform.position != Direction())
        {
            transform.position -= (Vector3)Direction() * Time.deltaTime;
        }
        else
        {
            Direction();
        }
    }
    Vector2 Direction()
    {
        x = UnityEngine.Random.Range(topL.position.x, bomR.position.x);
        y = UnityEngine.Random.Range(topL.position.y, bomR.position.y);
        return new Vector2(x, y);

    }
}
