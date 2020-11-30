using UnityEngine;

namespace Turret
{
    public class MachineGun : Weapons
    {

        private readonly TurretData _turretData;
        private ParticleSystem _muzzleFlash;
        private ObjectPoolManager _objectPoolManager = null;
        private float _timeToFire = 0.0f;

        public struct MachineGunProperties
        {
            public GameObject muzzleFlashVFX;
            public AudioSource audioSourceClips;
        }

        public MachineGunProperties MachineGunProps;

        public MachineGun(TurretData data) 
        {
            _turretData = data;
            _MaxAmmo = _turretData.machineGun.maxAmmo;
        }

        public override void SetFire(bool fire)
        {
            if (!(fire && (_currentAmmo > 0.0f) && (Time.time >= _timeToFire)))
            {
                if (_muzzleFlash.isPlaying) _muzzleFlash.Stop();
                return;
            }

            MachineGunProps.audioSourceClips.Play();
            if (!_muzzleFlash.isPlaying) _muzzleFlash.Play();
            _timeToFire = Time.time + (1f / _turretData.machineGun.fireRate);
            GameObject bullet = _objectPoolManager.GetObjectFromPool("Bullet");
            if (!bullet)
            {
                Debug.LogWarning("Missile Object Pool is Empty");
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
            _muzzleFlash = MachineGunProps.muzzleFlashVFX.GetComponentInChildren<ParticleSystem>();
            _muzzleFlash.Stop();
            MachineGunProps.audioSourceClips.clip = _turretData.machineGun.machinegunFire;
        }

    }

}