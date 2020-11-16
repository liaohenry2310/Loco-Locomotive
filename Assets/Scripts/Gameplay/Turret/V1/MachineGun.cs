using System.Collections.Generic;
using UnityEngine;

namespace Turret
{
    public class MachineGun : Weapons
    {

        private readonly TurretData _turretData;
        private ParticleSystem _muzzleFlash;
        private ObjectPoolManager _objectPoolManager = null;
        private float _timeToFire = 0.0f;

        public struct MachineGunVFXProperties
        {
            public GameObject muzzleFlashVFX;
        }

        public MachineGunVFXProperties MachineGunVFX;

        public MachineGun(TurretData data) 
        {
            _turretData = data;
        }

        public override void Reload()
        {
            _currentAmmo = _turretData.machineGun.maxAmmo;
        }

        public override void SetFire(bool fire)
        {
            if (!(fire && (_currentAmmo > 0.0f) && (Time.time >= _timeToFire)))
            {
                _muzzleFlash.Stop();
                return;
            }

            _muzzleFlash.Play();
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

        public override void SetUp(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _currentAmmo = _turretData.machineGun.maxAmmo;
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
            _muzzleFlash = MachineGunVFX.muzzleFlashVFX.GetComponentInChildren<ParticleSystem>();
            _muzzleFlash.Stop();
        }



        public override void SetUp(Transform spawmPoint, LineRenderer laserBeam)
        { }

        public override float CurretAmmo()
        {
            return _currentAmmo;
        }

    }

}