using Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Turret
{
    public class LaserBeam : Weapons
    {

        private LineRenderer _LaserBeam = null;
        private GameObject _StartVFX = null;
        private GameObject _EndVFX = null;
        private readonly List<ParticleSystem> _particles = new List<ParticleSystem>();

        public LaserBeam(TurretData data) : base(data)
        { }

        public override void Reload()
        {
            _currentAmmo = _turretData.laserGun.maxAmmo;
        }

        public override void SetFire()
        {
            if (_currentAmmo <= 0.0f)
            {
                DisableLaser();
                return;
            }

            if (!_LaserBeam.enabled)
            {
                EnableLaser();
            }

            _LaserBeam.SetPosition(0, (Vector2)_spawnPoint.position);
            _StartVFX.transform.position = (Vector2)_spawnPoint.position;

            //RaycastHit2D hit = Physics2D.Raycast(_spawnPoint.position, _spawnPoint.up, _turretData.laserGun.range, _turretData.laserGun.enemyLayerMask);
            RaycastHit2D hit = Physics2D.Raycast(_spawnPoint.position, _spawnPoint.up, _turretData.laserGun.range);//, _turretData.laserGun.enemyLayerMask);
            if (hit)
            {
                Collider2D collider = hit.collider;
                if (collider)
                {
                    _LaserBeam.SetPosition(1, hit.point);

                    //var shieldEnemy = collider.gameObject.GetComponentInChildren<EnemyShieldHealth>();
                    //if (shieldEnemy && shieldEnemy.ShieldIsActive)
                    //{
                    //    _LaserBeam.SetPosition(1, hit.point);
                    //}
                    //else
                    //{
                    //    _LaserBeam.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
                    //    IDamageableType<float> damageable = collider.GetComponent<EnemyHealth>();
                    //    if (damageable != null)
                    //    {
                    //        damageable.TakeDamage(_turretData.laserGun.damage * Time.deltaTime, DispenserData.Type.LaserBeam);
                    //    }
                    //}
                }
            }
            else
            {
                _LaserBeam.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
            }

            _EndVFX.transform.position = _LaserBeam.GetPosition(1);

            _currentAmmo -= _turretData.laserGun.ammoConsumeRate / Time.time;
            _currentAmmo = Mathf.Clamp(_currentAmmo, 0f, _turretData.laserGun.maxAmmo);
        }

        public override void SetUp(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _currentAmmo = _turretData.laserGun.maxAmmo;
        }

        public override void SetUp(Transform spawnPoint, LaserProperties laserBeam)
        {
            SetUp(spawnPoint);
            _LaserBeam = laserBeam.laserBeamRenderer;
            _StartVFX = laserBeam.startVFX;
            _EndVFX = laserBeam.endVFX;
            InitParticlesList();
            DisableLaser();
        }

        private void InitParticlesList()
        {
            for (int i = 0; i < _StartVFX.transform.childCount; ++i)
            {
                var ps = _StartVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps)
                {
                    _particles.Add(ps);
                }
            }

            for (int i = 0; i < _EndVFX.transform.childCount; ++i)
            {
                var ps = _EndVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps)
                {
                    _particles.Add(ps);
                }
            }
        }

        private void EnableLaser()
        {
            _LaserBeam.enabled = true;
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i].Play();
            }
        }


        public void DisableLaser()
        {
            _LaserBeam.enabled = false;
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i].Stop();
            }
        }



    }

}