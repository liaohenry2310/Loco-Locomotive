using UnityEngine;

namespace Turret
{
    public class EmpGun : Weapons
    {
        private readonly TurretData _turretData;
        private ParticleSystem _muzzleFlash = null;
        private ObjectPoolManager _objectPoolManager = null;
        private float _timeToFire = 0.0f;

        public struct EMPGunVFXProperties
        {
            public GameObject muzzleFlashVFX;
        }

        public EMPGunVFXProperties EmpGunVFX;

        public EmpGun(TurretData data)
        {
            _turretData = data;
        }

        public override void Reload()
        {
            _currentAmmo = _turretData.empShockWave.maxAmmo;
        }

        public override void SetFire(bool fire)
        {
            if (!(fire && (_currentAmmo > 0.0f) && (Time.time >= _timeToFire)))
            {
                if (_muzzleFlash.isPlaying) _muzzleFlash.Stop();
                return;
            }

            //TODO: Check why the particle are not emitting
            if (!_muzzleFlash.isPlaying) _muzzleFlash.Play();
            _timeToFire = Time.time + (1f / _turretData.empShockWave.fireRate);
            GameObject emp = _objectPoolManager.GetObjectFromPool("EMPShockWave");
            if (!emp)
            {
                Debug.LogWarning("EMPShockWave Object Pool is Empty");
                return;
            }
            emp.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            emp.SetActive(true);
            _currentAmmo--;
        }

        public override void SetUp(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _currentAmmo = _turretData.empShockWave.maxAmmo;
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
            _muzzleFlash = EmpGunVFX.muzzleFlashVFX.GetComponentInChildren<ParticleSystem>();
            _muzzleFlash.Stop();
        }
    }
}
