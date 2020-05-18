using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemies;
    



    private Camera MainCam;
    private Vector2 screenBounds;

    private float leftRange ;
    private float rightRange;
    private float spawnY;
    private int mEnemyNum;



    void Start()
    {
        MainCam = FindObjectOfType<Camera>();
        screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));

        // Function for Spawn Enemy. 
        InvokeRepeating("CreatEnemies", 2, 1.0f);
    }
    private void Update()
    {
        rightRange = screenBounds.x-1.0f;
        leftRange = -screenBounds.x+1.0f;
        spawnY = transform.position.y;

    }
    public void CreatEnemies()
    {
        
        mEnemyNum = Random.Range(0, 3);       
        for (int i = 0; i < mEnemyNum; i++)
        {
            float x;
            x = Random.Range(leftRange, rightRange);
            Instantiate(Enemies, new Vector3(x, spawnY, 0), Quaternion.identity);

        }
    }

}
