using Interfaces;
using UnityEngine;

namespace Turret
{
    public class LaserBeam : Weapons
    {

        private LineRenderer _LaserBeam = null;

        public LaserBeam(TurretData data): base(data)
        {}

        public override void Reload()
        {
            _currentAmmo = _turretData.laserGun.maxAmmo;
        }

        public override void SetFire()
        {
            if (_currentAmmo <= 0.0f)
            {
                _LaserBeam.enabled = false;
                return;
            }

            if (!_LaserBeam.enabled)
            {
                _LaserBeam.enabled = true;
            }

            RaycastHit2D hit = Physics2D.Raycast(_spawnPoint.position, _spawnPoint.up, _turretData.laserGun.range, _turretData.laserGun.enemyLayerMask);
            _LaserBeam.SetPosition(0, _spawnPoint.position);
            if (hit)
            {
                Collider2D collider = hit.collider;
                if (collider)
                {
                    var shieldEnemy = collider.gameObject.GetComponentInChildren<EnemyShieldHealth>();
                    if (shieldEnemy && shieldEnemy.ShieldIsActive)
                    {
                        _LaserBeam.SetPosition(1, hit.point);
                    }
                    else
                    {
                        _LaserBeam.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
                        IDamageableType<float> damageable = collider.GetComponent<EnemyHealth>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(_turretData.laserGun.damage * Time.deltaTime, DispenserData.Type.LaserBeam);
                        }
                    }
                }
            }
            else
            {
                _LaserBeam.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
            }
            _currentAmmo -= 1f / Time.time;
            _currentAmmo = Mathf.Clamp(_currentAmmo, 0f, _turretData.laserGun.maxAmmo);
        }

        public override void SetUp(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _currentAmmo = _turretData.laserGun.maxAmmo;
        }

        public override void SetUp(Transform spawnPoint, LineRenderer laserBeam)
        {
            SetUp(spawnPoint);
            _LaserBeam = laserBeam;
        }
    }

}