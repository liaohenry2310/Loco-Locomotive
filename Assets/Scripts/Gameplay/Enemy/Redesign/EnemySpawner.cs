using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public variables
    public EnemyWaveData enemyWaves;

    public Transform TopRight;
    public Transform BottomLeft;
    public GameObject SwarmEnemyGroup;
    private SwarmEnemyGroup _swarmEnemyGroup;

    //private variables
    private int _curretWaveIndex = 0;
    private float _nextWaveTime = 0.0f;
    private bool _spawnWormhole=false;


    //get list
    [NonSerialized] public List<GameObject> swarmNeighbors = new List<GameObject>();

    public List<GameObject> GetSwarmNeighbors () { return swarmNeighbors; }

    private ObjectPoolManager _objectPoolManager = null;
    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        _swarmEnemyGroup = Instantiate(SwarmEnemyGroup, transform).GetComponent<SwarmEnemyGroup>();
    }

    void Update()
    {
        if ((Time.time > _nextWaveTime) && (_curretWaveIndex < enemyWaves.waveData.Count))
          //  && _curretWaveIndex<=enemyWaves.waveData.Count())
        {
            UpdateWormholeDelay();
        }
        if (_spawnWormhole)
        {
            SpawnWave(enemyWaves.waveData[_curretWaveIndex-1]);
            _spawnWormhole = false;
        }
    }
    private void UpdateWormholeDelay()
    {
        
            var wave = enemyWaves.waveData[_curretWaveIndex++];
            _spawnWormhole = true;
            _nextWaveTime = Time.time + wave.NextWaveTimeDelay;
    }
    private void SpawnWave(EnemyWaveData.EnemyWave wave)
    {
        // (Optional) split the number of enemies into multiple wormholes for each wave.
        // 1. Get a new wormhole gameobject from the object pool.
        // 2. Reset the new wormhole and pass in the curret wave data.
        // 3. Pick a random spot between TopRight and BottomLeft Transforms and position the wormhole at that spot.
        // 4. Activate the new wormhole
        float x;
        float y;

        x = UnityEngine.Random.Range(BottomLeft.position.x+3.0f, TopRight.position.x- 3.0f);
        y = UnityEngine.Random.Range(TopRight.position.y - 3.0f, BottomLeft.position.y + 3.0f);
        GameObject _wormhole = _objectPoolManager.GetObjectFromPool("Wormhole");
        _wormhole.gameObject.GetComponent<Wormhole>().SetInitData(wave,TopRight,BottomLeft, _swarmEnemyGroup);
        _wormhole.transform.position = new Vector3(x, y, _wormhole.transform.position.z);
        //_wormhole = Instantiate(_wormhole, new Vector3(x, y, 0.0f), Quaternion.identity);
        _wormhole.SetActive(true);
    }
}
