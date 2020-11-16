using Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Turret
{
    public class LaserBeam : Weapons
    {
        private readonly TurretData _turretData;
        private readonly List<ParticleSystem> _particles;

        public struct LaserVFXProperties
        {
            public LineRenderer laserBeamRenderer;
            public GameObject startVFX;
            public GameObject endVFX;
        }

        public LaserVFXProperties LaserVFX;

        public LaserBeam(TurretData data)
        {
            _turretData = data;
            _particles = new List<ParticleSystem>();
        }

        public override void Reload()
        {
            _currentAmmo = _turretData.laserGun.maxAmmo;
        }

        public override void SetFire(bool fire)
        {
            if (fire && _currentAmmo > 0.0f)
            {
                if (!LaserVFX.laserBeamRenderer.enabled)
                {
                    EnableLaser();
                }

                LaserVFX.laserBeamRenderer.SetPosition(0, (Vector2)_spawnPoint.position);
                LaserVFX.startVFX.transform.position = (Vector2)_spawnPoint.position;
               
                RaycastHit2D hit = Physics2D.Raycast(_spawnPoint.position, _spawnPoint.up, _turretData.laserGun.range, _turretData.laserGun.enemyLayerMask);
                if (hit)
                {
                    Collider2D collider = hit.collider;
                    if (collider)
                    {
                        var shieldEnemy = collider.gameObject.GetComponentInChildren<EnemyShieldHealth>();
                        if (shieldEnemy && shieldEnemy.ShieldIsActive)
                        {
                            LaserVFX.laserBeamRenderer.SetPosition(1, hit.point);
                        }
                        else
                        {
                            LaserVFX.laserBeamRenderer.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
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
                    LaserVFX.laserBeamRenderer.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
                }

                LaserVFX.endVFX.transform.position = LaserVFX.laserBeamRenderer.GetPosition(1);

                _currentAmmo -= _turretData.laserGun.ammoConsumeRate / Time.time;
                _currentAmmo = Mathf.Clamp(_currentAmmo, 0f, _turretData.laserGun.maxAmmo);
            }
            else
            {
                DisableLaser();
            }
        }

        public override void SetUp(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _currentAmmo = _turretData.laserGun.maxAmmo;
            InitParticlesList();
            DisableLaser();
        }

        private void InitParticlesList()
        {
            for (int i = 0; i < LaserVFX.startVFX.transform.childCount; ++i)
            {
                var ps = LaserVFX.startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps)
                {
                    _particles.Add(ps);
                }
            }

            for (int i = 0; i < LaserVFX.endVFX.transform.childCount; ++i)
            {
                var ps = LaserVFX.endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps)
                {
                    _particles.Add(ps);
                }
            }
        }


        private void EnableLaser()
        {
            LaserVFX.laserBeamRenderer.enabled = true;
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i].Play();
            }
        }


        public void DisableLaser()
        {
            LaserVFX.laserBeamRenderer.enabled = false;
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i].Stop();
            }
        }

    }

}