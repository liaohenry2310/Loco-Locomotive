using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public variables
    public EnemyWaveData enemyWaves;

    public Transform TopRight;
    public Transform BottomLeft;

    //private variables
    private int _curretWaveIndex = 0;
    private float _nextWaveTime = 0.0f;

    private ObjectPoolManager _objectPoolManager = null;
    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }
    void Start()
    {
        


    }

    void Update()
    {
        if ((Time.time > _nextWaveTime) )
          //  && _curretWaveIndex<=enemyWaves.waveData.Count())
        {
            var wave = enemyWaves.waveData[_curretWaveIndex++];
            SpawnWave(wave);
            _nextWaveTime = Time.time + wave.TimeDelay;
        }
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

        x = UnityEngine.Random.Range(TopRight.position.x, BottomLeft.position.x);
        y = UnityEngine.Random.Range(TopRight.position.y, BottomLeft.position.y);
        GameObject _wormhole = _objectPoolManager.GetObjectFromPool("Wormhole");
        _wormhole.gameObject.GetComponent<Wormhole>().SetInitData(wave);
        _wormhole.transform.position = new Vector3(x, y, _wormhole.transform.position.z);
        //_wormhole = Instantiate(_wormhole, new Vector3(x, y, 0.0f), Quaternion.identity);
        _wormhole.SetActive(true);
    }
}
