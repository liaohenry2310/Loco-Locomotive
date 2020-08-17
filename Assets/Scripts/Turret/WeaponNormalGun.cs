using UnityEngine;

public class WeaponNormalGun : MonoBehaviour
{
    [SerializeField] private Transform _cannonFirePoint = default;
    [SerializeField] private int _currentAmmo = 0;
    [SerializeField] private int _maxAmmo = 100;
    [SerializeField] private float _fireRate = 0f;
    [SerializeField] private float _spreadBulletFactor = 3f;

    private ObjectPoolManager _objectPoolManager = null;
    private float _timeToFire = 0f;

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

        GameObject bullet = _objectPoolManager.GetObjectFromPool("Bullet");
        if (!bullet)
        {
            Debug.LogWarning("Bullet Object Pool is Empty");
            return;
        }

        bullet.transform.SetPositionAndRotation(_cannonFirePoint.transform.position,
            Quaternion.RotateTowards(_cannonFirePoint.transform.rotation, Random.rotation, _spreadBulletFactor));
        bullet.SetActive(true);
        _currentAmmo--;
    }

    public void Reload() => _currentAmmo = _maxAmmo;

}
