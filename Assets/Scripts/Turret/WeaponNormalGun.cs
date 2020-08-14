using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class WeaponNormalGun : MonoBehaviour
{
    [SerializeField] private Transform _cannonFirePoint = default;
    [SerializeField] private int _currentAmmo = 0;
    [SerializeField] private float _fireRate = 0f;
    [SerializeField] private float _spreadBulletFactor = 3f;

    private ObjectPooler bulletPooler;
    private float _timeToFire = 0f;

    public int CurrentAmmo
    {
        get => _currentAmmo;
        private set { }
    }

    void Start()
    {
        _ = TryGetComponent(out bulletPooler);
        _currentAmmo = bulletPooler.AmountToPool;
    }

    public void SetFire(bool isTrigger)
    {
        if (isTrigger && (_currentAmmo > 0) && (Time.time >= _timeToFire))
        {
            _timeToFire = Time.time + (1f / _fireRate);
            // bck original
            //var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
            //var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
            //mCurrentAmmo--;
            //x.transform.rotation = Quaternion.RotateTowards(CannonFirePoint.transform.rotation, Random.rotation, spreadFactor);
            // bck original
            //x.transform.rotation = CannonFirePoint.rotation;
            //AmmoCountText.text = $"Ammo: {--mCurrentAmmo}";

            // novo teste
            GameObject bullet = bulletPooler.GetPooledObject();
            if (bullet)
            {
                bullet.transform.position = _cannonFirePoint.transform.position;
                bullet.transform.rotation = Quaternion.RotateTowards(_cannonFirePoint.transform.rotation, Random.rotation, _spreadBulletFactor);
                bullet.SetActive(true);
                _currentAmmo--;
            }
        }
    }

    public void Reload() => _currentAmmo = bulletPooler.AmountToPool;

}
