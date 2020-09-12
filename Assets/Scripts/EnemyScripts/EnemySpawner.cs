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
    private float wormholeSpawnTime = 0.0f;
    private bool enabledWormhole;
    public Transform topL, bomR;
    // Bounding Check
    private Camera MainCam;
    private Vector2 screenBounds;
    private float leftRange;
    private float rightRange;
    private float spawnY;
    private int mEnemyNum;



    //int numOfEnemis;
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
        StartCoroutine(CreatEnemies(_wave._waves[currentWave]));
    }
    private void Update()
    {
        if (_wave._waves[currentWave].numOfEnemies <= currentNumber && _wave._waves.Count > currentWave+1 )
        {
            ++currentWave;
            currentNumber = 0;
            StartCoroutine(CreatEnemies(_wave._waves[currentWave]));
        }



    }

    public IEnumerator CreatEnemies(EnemyLevel.EnemyIniti enemyInit)
    {


        yield return new WaitForSeconds(enemyInit.wave_delay);

        float x;
        float y;

        x = UnityEngine.Random.Range(topL.position.x, bomR.position.x);
        y = UnityEngine.Random.Range(topL.position.y, bomR.position.y);
        GameObject go= Instantiate(wormhole, new Vector3(x,y,0.0f), Quaternion.identity); ;
        go.transform.position = new Vector3(x, y,wormhole.transform.position.z);

        Wormhole wormholeComponent = go.GetComponent<Wormhole>();
        wormholeComponent.wormholeDuration += Time.time;
        wormholeComponent.wormholeRSpeed = enemyInit.wormholeRotationSpeed;
        wormholeComponent.wormholeGrowthRate =enemyInit.wormholeGrowthRate;

        EnemyType enemyType = enemyInit.enemyType;
        int numOfEnemis = enemyInit.numOfEnemies;
        float enemySpawnDelay = enemyInit.wave_delay;


        StartCoroutine(helloEnemy(numOfEnemis, wormholeSpawnTime, enemyType,go.transform.position));


    }
    public IEnumerator helloEnemy(int numOfEnemis, float timedelay, EnemyType enemyType,Vector3 spawnPos)
    {

        yield return new WaitForSeconds(timedelay+1.0f);

        for (int i = 0; i < numOfEnemis; ++i)
        {

            GameObject enemy = null;
            enemy = Instantiate(EnemyPrefab[enemyType], spawnPos, Quaternion.identity);

            enemy.GetComponent<Enemy>().targetList.AddRange(targetList);
            enemy.GetComponent<Enemy>().landingCollider = landingCollider;
            enemy.GetComponent<Enemy>().trainArea = trainArea;
            ++currentNumber;
        }

    }


}