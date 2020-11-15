using UnityEngine;

namespace Turret
{
    public class MachineGun : Weapons
    {

        private float _timeToFire = 0.0f;

        public MachineGun(TurretData data) : base(data)
        { }

        public override void Reload()
        {
            _currentAmmo = _turretData.machineGun.maxAmmo;
        }

        public override void SetFire()
        {
            if (!((_currentAmmo > 0.0f) && (Time.time >= _timeToFire))) return;

            _timeToFire = Time.time + (1f / _turretData.machineGun.fireRate);

            GameObject bullet = _objectPoolManager.GetObjectFromPool("Bullet");
            if (!bullet)
            {
                Debug.LogWarning("Bullet Object Pool is Empty");
                return;
            }
            Quaternion rotation = Quaternion.RotateTowards(_spawnPoint.rotation, Random.rotation, _turretData.machineGun.spreadBullet);
            bullet.transform.SetPositionAndRotation(_spawnPoint.position, rotation);
            bullet.SetActive(true);
            _currentAmmo--;
        }

        public override void SetUp(Transform spawmPoint)
        {
            _spawnPoint = spawmPoint;
            _currentAmmo = _turretData.machineGun.maxAmmo;
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        }


        public override void SetUp(Transform spawmPoint, LineRenderer laserBeam)
        { }

        public override float CurretAmmo()
        {
            return _currentAmmo;
        }
    }

}