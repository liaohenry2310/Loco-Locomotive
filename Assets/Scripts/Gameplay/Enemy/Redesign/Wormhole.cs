using UnityEngine;

public class Wormhole : MonoBehaviour
{
    [SerializeField] private ParticleSystem _VFX = null;
    [SerializeField] private float _spinSpeed = 5.0f;
    [SerializeField] private float _scaleDeltaSpeed = 0.1f;
    [SerializeField] private GameObject _sound = null;

    private EnemyWaveData.EnemyWave _waveData;
    private float _currentScale = 0.0f;
    private int _currentSpawned = 0;
    private float _maxScale = 1.0f;
    private bool _spawnedEnemy = false;

    private ObjectPoolManager _objectPoolManager = null;
    private Transform _topright;
    private Transform _bottomLeft;
    private SwarmEnemyGroup _swarmEnemyGroup;

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }

    public void SetInitData(EnemyWaveData.EnemyWave wave, Transform topRight, Transform bottomLeft, SwarmEnemyGroup swarmEnemyGroup)
    {
        _waveData = wave;
        _maxScale = wave.WormholeSize;
        _currentScale = 0.0f;
        _spawnedEnemy = false;
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        _topright = topRight;
        _bottomLeft = bottomLeft;
        _swarmEnemyGroup = swarmEnemyGroup;
    }

    void Update()
    {
        //_currentScale = transform.localScale.x;
        // Growing and shrinking the wormhole
        if (_currentScale < _maxScale)
        {
            transform.Rotate(0.0f, 0.0f, _spinSpeed);
            transform.localScale += new Vector3(transform.localScale.x, transform.localScale.y, 1f) * Time.deltaTime;
            _currentScale = transform.localScale.x;
            PlayParticle();

            if (_currentScale >= _maxScale / 2 && !_spawnedEnemy && (_currentSpawned < _waveData.NumToSpawn))
            {
                SpawnEnemies();
                _spawnedEnemy = true;
            }
        }
        else
        {
            transform.Rotate(0.0f, 0.0f, _spinSpeed);
            transform.localScale -= new Vector3(transform.localScale.x, transform.localScale.y, 1f) * Time.deltaTime;
            
            if ((transform.localScale.x <= 0.02f))
            {
                _spawnedEnemy = false;
                RecycleWormhole();
            }
        }
    }

    public void PlayParticle()
    {
        ParticleSystem particle = Instantiate(_VFX, transform.position, Quaternion.identity);
        Instantiate(_sound, gameObject.transform.position, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration);
    }

    void SpawnEnemies()
    {
        GameObject _enemyType;
        if (_waveData.EnemyType == EnemyWaveData.EnemyType.Swarm)
        {
            _swarmEnemyGroup.transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.z);
            _swarmEnemyGroup.gameObject.GetComponent<SwarmEnemyGroup>().SetNewData(_topright, _bottomLeft);
            _swarmEnemyGroup.gameObject.GetComponent<SwarmEnemyGroup>().SetSwarmSpawnPos(transform);
            _swarmEnemyGroup.gameObject.GetComponent<SwarmEnemyGroup>().SpawnGroup(_waveData.NumToSpawn, false);

            return;
        }
        if (_waveData.EnemyType == EnemyWaveData.EnemyType.Swarm_Shield)
        {
            _swarmEnemyGroup.transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.z);
            _swarmEnemyGroup.gameObject.GetComponent<SwarmEnemyGroup>().SetNewData(_topright, _bottomLeft);
            _swarmEnemyGroup.gameObject.GetComponent<SwarmEnemyGroup>().SetSwarmSpawnPos(transform);
            _swarmEnemyGroup.gameObject.GetComponent<SwarmEnemyGroup>().SpawnGroup(_waveData.NumToSpawn, true);

            return;
        }

        for (_currentSpawned = 0; _currentSpawned < _waveData.NumToSpawn; ++_currentSpawned)
        {

            switch (_waveData.EnemyType)
            {
                case EnemyWaveData.EnemyType.Basic:
                    {
                         _enemyType = _objectPoolManager.GetObjectFromPool("BasicEnemy");
                        _enemyType.transform.position = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), transform.localPosition.z);
                        _enemyType.SetActive(true);
                        _enemyType.gameObject.GetComponent<BasicEnemy>().SetNewData(_topright, _bottomLeft);
                        break;
                    }
                case EnemyWaveData.EnemyType.Bomber:
                {
                    _enemyType = _objectPoolManager.GetObjectFromPool("BomberEnemy");
                    _enemyType.transform.position = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), transform.localPosition.z);
                    _enemyType.SetActive(true);
                    _enemyType.gameObject.GetComponent<BomberEnemy>().SetNewData(_topright, _bottomLeft);
                    break;
                }
                case EnemyWaveData.EnemyType.Giant:
                {
                    _enemyType = _objectPoolManager.GetObjectFromPool("GiantEnemy");
                    _enemyType.transform.position = new Vector3(transform.position.x , transform.position.y, transform.localPosition.z);
                    _enemyType.SetActive(true);
                    _enemyType.gameObject.GetComponent<GiantEnemy>().SetNewData(_topright, _bottomLeft);
                    break;
                }
                case EnemyWaveData.EnemyType.Basic_Shield:
                {
                    _enemyType = _objectPoolManager.GetObjectFromPool("Basic_Shield");
                    _enemyType.transform.position = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), transform.localPosition.z);
                    _enemyType.SetActive(true);
                    _enemyType.gameObject.GetComponent<BasicEnemy>().SetNewData(_topright, _bottomLeft);
                    break;
                }
                case EnemyWaveData.EnemyType.Bomber_Shield :
                {
                    _enemyType = _objectPoolManager.GetObjectFromPool("Bomber_Shield");
                    _enemyType.transform.position = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), transform.localPosition.z);
                    _enemyType.SetActive(true);
                    _enemyType.gameObject.GetComponent<BomberEnemy>().SetNewData(_topright, _bottomLeft);
                    break;
                }
                case EnemyWaveData.EnemyType.Giant_Shield:
                {
                    _enemyType = _objectPoolManager.GetObjectFromPool("Giant_Shield");
                    _enemyType.transform.position = new Vector3(transform.position.x , transform.position.y , transform.localPosition.z);
                    _enemyType.SetActive(true);
                    _enemyType.gameObject.GetComponent<GiantEnemy>().SetNewData(_topright, _bottomLeft);
                    break;
                }

                default:
                    break;
            }

            //Pick a random spot inside the wormhole.
            //Spawn the desired enemy prefab, from the object pool, at the random spot.
        }

        _currentSpawned = 0;
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