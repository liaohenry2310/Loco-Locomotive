using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemies;
    void Start()
    {

        InvokeRepeating("CreatEnemies", 1, 1);

    }
    public void CreatEnemies()
    {
        int EnemyNum;
        EnemyNum = Random.Range(0, 3);       
        for (int i = 0; i < EnemyNum; i++)
        {
            float x;
            x = Random.Range(-6, 6);
            Instantiate(Enemies, new Vector3(x, 2.8f, 0), Quaternion.identity);

        }

    }

}
