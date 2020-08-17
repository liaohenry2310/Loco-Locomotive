using UnityEngine;

public class WeaponMissile : MonoBehaviour
{
    [SerializeField] private Transform _cannonFirePoint = default;
    [SerializeField] private int _currentAmmo = 0;
    [SerializeField] private int _maxAmmo = 100;
    [SerializeField] private float _fireRate = 0f;
    [SerializeField] private float _radiusAreaEffect = 0f;

    private ObjectPoolManager _objectPoolManager = null;
    private float _timeToFire = 0f;

    public float RadiusAreaEffect => _radiusAreaEffect;

    public int CurrentAmmo
    {
        get => _currentAmmo;
        private set { }
    }

    private void Awake()
    {
        _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
    }

    private void Start()
    {
        _currentAmmo = _maxAmmo;
    }

    public void SetFire(bool isTrigger)
    {
        if (!(isTrigger && (_currentAmmo > 0) && (Time.time >= _timeToFire))) return;
        _timeToFire = Time.time + (1f / _fireRate);
        // bck original
        //var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
        //var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
        //mCurrentAmmo--;
        //x.transform.rotation = Quaternion.RotateTowards(CannonFirePoint.transform.rotation, Random.rotation, spreadFactor);
        // bck original
        //x.transform.rotation = CannonFirePoint.rotation;
        //AmmoCountText.text = $"Ammo: {--mCurrentAmmo}";

        GameObject missile = _objectPoolManager.GetObjectFromPool("Missile");
        if (!missile)
        {
            Debug.LogWarning("Bullet Object Pool is Empty");
            return;
        }
        missile.transform.SetPositionAndRotation(_cannonFirePoint.transform.position,
            _cannonFirePoint.transform.rotation);
        missile.SetActive(true);
        missile.GetComponent<Missile>().AreaOfEffect = _radiusAreaEffect;
        _currentAmmo--;
    }

    public void Reload() => _currentAmmo = _maxAmmo;

}