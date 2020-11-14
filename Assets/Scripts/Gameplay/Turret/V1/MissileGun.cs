using UnityEngine;

namespace Turret
{
    public class MissileGun : Weapons
    {
        private readonly TurretData _turretData;
        private ObjectPoolManager _objectPoolManager = null;
        private float _timeToFire = 0.0f;

        public MissileGun(TurretData data)
        {
            _turretData = data;
        }

        public override void Reload()
        {
            _currentAmmo = _turretData.missileGun.maxAmmo;
        }

        public override void SetFire(bool fire)
        {
            if (!(fire && (_currentAmmo > 0) && (Time.time >= _timeToFire))) return;

            _timeToFire = Time.time + (1f / _turretData.missileGun.fireRate);
            GameObject missile = _objectPoolManager.GetObjectFromPool("Missile");
            if (!missile)
            {
                Debug.LogWarning("Bullet Object Pool is Empty");
                return;
            }
            missile.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            missile.SetActive(true);
            _currentAmmo--;
        }

        public override void SetUp(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _currentAmmo = _turretData.missileGun.maxAmmo;
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }

    }

}
