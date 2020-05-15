using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemies;
    public float leftRange = -8.0f;
    public float rightRange = 8.0f;
    void Start()
    {
        InvokeRepeating("CreatEnemies", 2, 1.0f);
    }
    public void CreatEnemies()
    {
        int EnemyNum;
        EnemyNum = Random.Range(0, 3);       
        for (int i = 0; i < EnemyNum; i++)
        {
            float x;
            x = Random.Range(leftRange, rightRange);
            Instantiate(Enemies, new Vector3(x, 2.8f, 0), Quaternion.identity);

        }

    }

}
