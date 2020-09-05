using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemySpawner : MonoBehaviour
{
    public enum EnemyType
    {
        None = 0,
        Enemy_Basic,
        Enemy_ArmorBasic,
        Enemy_ShieldBasic,
        Enemy_ShieldArmorBasic,
        Enemy_Rider,
        Enemy_ArmorRider,
        Enemy_ShieldRider,
        Enemy_ShieldArmorRider,
        Enemy_SwarmGroup
    }
    [Serializable]
    public class EnemiesPrefabObject
    {
        public EnemyType enemyType;
        public GameObject prefab;
    }

    public List<EnemiesPrefabObject> EnemiesPrefabList = new List<EnemiesPrefabObject>();
    Dictionary<EnemyType, GameObject> EnemyPrefab = new Dictionary<EnemyType, GameObject>();




    [Header("Attributes")]
    [SerializeField] private EnemyLevel _wave = default;
    int currentWave = 0;
    int currentNumber = 0;

    public List<GameObject> targetList;
    public GameObject landingCollider;
    public GameObject trainArea;
    public GameObject wormhole;
    public float wormholeRSpeed = 0.0f;
    public float wormholegrowthRate = 0.5f;
    public float wormholeSpawnTime = 0.0f;
    public Transform topL, bomR;
    // Bounding Check
    private Camera MainCam;
    private Vector2 screenBounds;
    private float leftRange;
    private float rightRange;
    private float spawnY;
    private int mEnemyNum;
    private bool enabledWormhole;

    private float enemySpawnDelay = 0.0f;
    float currentTime;
    void Start()
    {
        //Bounding Check
        screenBounds = GameManager.GetScreenBounds;
        rightRange = screenBounds.x + 5.0f;
        leftRange = -screenBounds.x + 1.0f;
        // Function for Spawn Enemy. 
        //   InvokeRepeating("CreatEnemies", 2, 1.0f);


        foreach (var prefab in EnemiesPrefabList)
        {
            if (!EnemyPrefab.ContainsKey(prefab.enemyType))
            {
                EnemyPrefab.Add(prefab.enemyType, prefab.prefab);
            }
        }
        wormhole.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        enabledWormhole = false;
        StartCoroutine(CreatEnemies());

    }
    private void Update()
    {
        spawnY = transform.position.y;
        if (_wave._waves[currentWave].numOfEnemies <= currentNumber && _wave._waves.Count > currentWave + 1 && !enabledWormhole)
        {
            currentWave++;
            currentNumber = 0;
            StartCoroutine(CreatEnemies());
        }
        if (enabledWormhole)
        {
            if (_wave._waves[currentWave].numOfEnemies <= currentNumber)
            {
                WormholeDispear();
            }
            else
            {
                Wormhole();
            }
        }

    }

    public IEnumerator CreatEnemies()
    {
        EnemyType enemyType;
        int numOfEnemis;

        enemyType = _wave._waves[currentWave].enemyType;
        numOfEnemis = _wave._waves[currentWave].numOfEnemies;
        enemySpawnDelay = _wave._waves[currentWave].wave_delay;

        yield return new WaitForSeconds(enemySpawnDelay);
        enabledWormhole = true;
        //wormhole randpos;
        float x;
        float y;

        x = UnityEngine.Random.Range(topL.position.x, bomR.position.x);
        y = UnityEngine.Random.Range(topL.position.y, bomR.position.y);
        wormhole.gameObject.transform.position = new Vector3(x, y, 0.0f);

        StartCoroutine(helloEnemy(numOfEnemis, wormholeSpawnTime, enemyType));


    }
    public IEnumerator helloEnemy(int numOfEnemis, float timedelay, EnemyType enemyType)
    {

        yield return new WaitForSeconds(timedelay);

        List<GameObject> swarmNeighbors = new List<GameObject>();

        for (int i = 0; i < numOfEnemis; ++i)
        {

            GameObject enemy = null;
            enemy = Instantiate(EnemyPrefab[enemyType], wormhole.transform.position, Quaternion.identity);

            //if (enemyType == EnemyType.Enemy_SwarmGroup)
            //{
            //    SwarmEnemy swarmEnemy = enemy.GetComponent<SwarmEnemy>();
            //    swarmNeighbors.Add(enemy);
            //    for (int j = 0; j < swarmEnemy.GroupOfSize; ++j)
            //    {
            //        swarmNeighbors.Add(Instantiate(enemy));
            //    }
            //    for (int k = 0; k < swarmNeighbors.Count; ++k)
            //    {
            //        swarmNeighbors[k].GetComponent<SwarmEnemy>().swarmNeighbors = swarmNeighbors;
            //        swarmNeighbors[k].GetComponent<Enemy>().targetList.AddRange(targetList);
            //        swarmNeighbors[k].GetComponent<Enemy>().landingCollider = landingCollider;
            //        swarmNeighbors[k].GetComponent<Enemy>().trainArea = trainArea;
            //    }
            //    ++currentNumber;
            //    continue;
            //}
            enemy.GetComponent<Enemy>().targetList.AddRange(targetList);
            enemy.GetComponent<Enemy>().landingCollider = landingCollider;
            enemy.GetComponent<Enemy>().trainArea = trainArea;
            ++currentNumber;
        }

    }

    public void Wormhole()
    {

        wormhole.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        wormhole.transform.Rotate(0.0f, 0.0f, wormholeRSpeed);
        wormhole.transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * wormholegrowthRate;
    }

    public void WormholeDispear()
    {

        wormhole.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        wormhole.transform.Rotate(0.0f, 0.0f, wormholeRSpeed);
        wormhole.transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime * wormholegrowthRate;
        if (wormhole.transform.localScale.x <= 0.0f)
        {
            enabledWormhole = false;
            wormhole.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}