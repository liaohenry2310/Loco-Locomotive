using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 5.0f;
    [SerializeField] private float _scaleDeltaSpeed = 0.1f;

    private EnemyWaveData.EnemyWave _waveData;
    private float _currentScale = 0.0f;
    private float _maxScale = 1.0f;
    private bool _spawnedEnemy = false;
    private Vector3 _screenBounds;
    private ObjectPoolManager _objectPoolManager = null;

    //private void Start()
    //{
    //    _screenBounds = GameManager.GetScreenBounds;
    //}
    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }

    public void SetInitData(EnemyWaveData.EnemyWave wave)
    {
        _waveData = wave;
        _maxScale = wave.WormholeSize;
        _currentScale = 0.0f;
        _spawnedEnemy = false;
    }

    void Update()
    {
        //_currentScale = transform.localScale.x;
        // Growing and shrinking the wormhole
        if (_currentScale < _maxScale)
        {

            transform.Rotate(0.0f, 0.0f, _spinSpeed);
            transform.localScale += new Vector3(transform.localScale.x, transform.localScale.y, 1f) * Time.deltaTime;//*_scaleDeltaSpeed*_spinSpeed ;
            _currentScale = transform.localScale.x;

            if (_currentScale >= _maxScale/2)
            {
                _spawnedEnemy = true;
            }
        }
        else   {
            transform.Rotate(0.0f, 0.0f, _spinSpeed);
            transform.localScale -= new Vector3(transform.localScale.x, transform.localScale.y, 1f) * Time.deltaTime;// * _scaleDeltaSpeed * _spinSpeed;
            if ((_currentScale <= 0.0f) )
            {
                RecycleWormhole();
            }
        }
        if (_spawnedEnemy)
        {
           SpawnEnemies();
        }



    }

    void SpawnEnemies()
    {

        for (int i = 0; i < _waveData.NumToSpawn; ++i)
       {
            
        //GameObject _enemyType;
        switch (_waveData.EnemyType)
        {
            case EnemyWaveData.EnemyType.Basic:
                {
                    GameObject _enemyType = _objectPoolManager.GetObjectFromPool("BasicEnemy");
                    _enemyType.gameObject.GetComponent<BasicEnemy>().SetNewData();// (null);
                    _enemyType.transform.position = new Vector3(transform.position.x,transform.position.y,transform.localPosition.z);
                    _enemyType.SetActive(true);
                        break;
                }
            case EnemyWaveData.EnemyType.Swarm:
                //_enemyType = _objectPoolManager.GetObjectFromPool("SwarmEnemy");
                break;
            default:
                break;
        }
           //Pick a random spot inside the wormhole.
           //Spawn the desired enemy prefab, from the object pool, at the random spot.
       }
    }


    private void RecycleWormhole()
    {
        if (_objectPoolManager == null)
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }
        _objectPoolManager.RecycleObject(gameObject);
    }
}