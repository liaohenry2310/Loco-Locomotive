using UnityEngine;

namespace Turret
{
    public class ShieldGun : Weapons
    {
        private readonly TurretData _turretData;

        public struct ShieldGunProperties
        {
            public ShieldGunController shieldGunController;
        }
        
        public ShieldGunProperties ShieldGunPros;

        public ShieldGun(TurretData data)
        {
            _turretData = data;
            _MaxAmmo = _turretData.shieldGun.maxAmmo;
        }

        public override void SetFire(bool fire)
        {
            if (fire && _currentAmmo > 0.0f)
            {
                EnableShield();
                _currentAmmo -= _turretData.shieldGun.ammoConsumeRate / Time.time;
                _currentAmmo = Mathf.Clamp(_currentAmmo, 0.0f, _MaxAmmo);
            }
            else
            {
                DisableShield();
            }
        }

        private void EnableShield()
        {
            ShieldGunPros.shieldGunController.EnabledShield(true);
        }

        private void DisableShield()
        {
            ShieldGunPros.shieldGunController.EnabledShield(false);
        }

        public override void SetUp(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _currentAmmo = _turretData.shieldGun.maxAmmo;
            DisableShield();
        }
    }

}