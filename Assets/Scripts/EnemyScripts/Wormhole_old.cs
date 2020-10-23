using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole_old : MonoBehaviour
{

    public float wormholeRSpeed = 0.0f;
    public float wormholeGrowthRate = 0.5f;
    public float wormholeDuration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (wormholeDuration<=Time.time)
        {
            WormholeDispear();
        }
        else
        {
            WormholeSpawner();
        }
    }


    public void WormholeSpawner()
    {

        transform.Rotate(0.0f, 0.0f, wormholeRSpeed);
        transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * wormholeGrowthRate;
    }

    public void WormholeDispear()
    {
        transform.Rotate(0.0f, 0.0f, wormholeRSpeed);
        transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime * wormholeGrowthRate;
        if (transform.localScale.x <= 0.0f)
        {
            Destroy(gameObject);

        }
    }
}
