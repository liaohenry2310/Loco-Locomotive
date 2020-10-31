using UnityEngine;

namespace Turret
{
    public class MissileGun : Weapons
    {
        private float _timeToFire = 0.0f;

        public MissileGun(TurretData data) : base(data)
        { }

        public override void Reload()
        {
            _currentAmmo = _turretData.missileGun.maxAmmo;
        }

        public override void SetFire()
        {
            if (!((_currentAmmo > 0) && (Time.time >= _timeToFire))) return;
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

        public override void SetUp(Transform spawnPoint, LineRenderer laserBeam)
        { }

    }

}
