using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemies;
    public List<GameObject> targetList;
    // public GameObject groundArea;
    public GameObject topWagonCollider;
    public GameObject trainArea;
    // Bounding Check
    private Camera MainCam;
    private Vector2 screenBounds;
    private float leftRange;
    private float rightRange;
    private float spawnY;
    private int mEnemyNum;

    void Start()
    {
        //Bounding Check
        MainCam = FindObjectOfType<Camera>();
        screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
        rightRange = screenBounds.x + 5.0f;
        leftRange = -screenBounds.x + 1.0f;
        // Function for Spawn Enemy. 
        InvokeRepeating("CreatEnemies", 2, 1.0f);
    }
    private void Update()
    {
        spawnY = transform.position.y;
    }
    public void CreatEnemies()
    {

        mEnemyNum = Random.Range(0, 2);
        for (int i = 0; i < mEnemyNum; i++)
        {
            float x;
            x = Random.Range(leftRange, rightRange);
            GameObject enemy = Instantiate(Enemies, new Vector3(x, spawnY, 0), Quaternion.identity);
            if (enemy.GetComponent<RiderEnemy>())
            {
                enemy.GetComponent<RiderEnemy>().targetList.AddRange(targetList);
                // enemy.GetComponent<RiderEnemy>().groundArea = groundArea;
                enemy.GetComponent<RiderEnemy>().topWagonCollider = topWagonCollider;
            }
            if (enemy.GetComponent<BasicEnemy>())
            {
                enemy.GetComponent<BasicEnemy>().trainArea = trainArea;
            }
        }
    }
}


