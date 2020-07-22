using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float health;

    public float damage=10.0f;

    public float speed = 1.0f;

    public float windSpeed = 1.5f;

    public GameObject trainArea;

    EnemyProjectilesManager mProjectiles;

    public float shootingDelay= 1.5f;

    float trainWidgh;
    float trainHeight;


    void Start()
    {
        trainWidgh = trainArea.GetComponentInChildren<BoxCollider2D>().size.x;
        trainHeight = trainArea.GetComponentInChildren<BoxCollider2D>().size.y;
        mProjectiles = GameObject.FindObjectOfType<EnemyProjectilesManager>().GetComponent<EnemyProjectilesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingDelay<Time.time)
        {
            shootingDelay = Time.time + 3.5f;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        float randomNumX = Random.Range(trainArea.transform.position.x - trainWidgh*0.5f, trainArea.transform.position.x + trainWidgh *0.5f);
        float randomNumY = Random.Range(trainArea.transform.position.y - trainHeight*0.5f, trainArea.transform.position.y + trainHeight * 0.5f);

        GameObject projectile = mProjectiles.GetActiveProjectiles();
        projectile.GetComponent<BasicEnemyProjectile>().targetPos = new Vector2(randomNumX,randomNumY);
        projectile.GetComponent<BasicEnemyProjectile>().damage = damage;
        projectile.SetActive(true);

        projectile.transform.position = gameObject.transform.position;

    }
}
