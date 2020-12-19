using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turret
{
    public class LaserBeam : Weapons
    {
        private readonly TurretData _turretData;
        private readonly List<ParticleSystem> _particles;
        private ParticleSystem _hitVFX = null;

        public struct LaserGunProperties
        {
            public LineRenderer laserBeamRenderer;
            public GameObject startVFX;
            public GameObject endVFX;
            public GameObject hitVFX;
            public AudioSource audioSourceClips;
            public MonoBehaviour monoBehaviour;
        }

        public LaserGunProperties LaserGunProps;

        public LaserBeam(TurretData data)
        {
            _turretData = data;
            _particles = new List<ParticleSystem>();
            _MaxAmmo = _turretData.laserGun.maxAmmo;
        }

        public override void SetFire(bool fire)
        {
            if (fire && _currentAmmo > 0.0f)
            {
                if (!LaserGunProps.laserBeamRenderer.enabled)
                {
                    EnableLaser();
                }

                LaserGunProps.laserBeamRenderer.SetPosition(0, (Vector2)_spawnPoint.position);
                LaserGunProps.startVFX.transform.position = (Vector2)_spawnPoint.position;

                RaycastHit2D[] hit = Physics2D.RaycastAll(_spawnPoint.position, _spawnPoint.up, _turretData.laserGun.range, _turretData.laserGun.enemyLayerMask);
                for (int i = 0; i < hit.Length; ++i)
                {
                    RaycastHit2D enemyHit = hit[i];
                    if (enemyHit)
                    {
                        Collider2D collider = enemyHit.collider;
                        if (collider)
                        {
                            var shieldEnemy = collider.gameObject.GetComponentInChildren<EnemyShieldHealth>();
                            if (shieldEnemy && shieldEnemy.ShieldIsActive)
                            {
                                LaserGunProps.laserBeamRenderer.SetPosition(1, enemyHit.point);
                                LaserGunProps.endVFX.transform.position = LaserGunProps.laserBeamRenderer.GetPosition(1);
                            }
                            else
                            {
                                LaserGunProps.laserBeamRenderer.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
                                IDamageableType<float> damageable = collider.GetComponent<EnemyHealth>();
                                if (damageable != null)
                                {
                                    damageable.TakeDamage(_turretData.laserGun.damage * Time.deltaTime, DispenserData.Type.LaserBeam);
                                    ParticleSystem hitParticle = Object.Instantiate(_hitVFX, enemyHit.point, Quaternion.identity);
                                    hitParticle.Play();
                                    Object.Destroy(hitParticle.gameObject, 0.05f);
                                }

                            }
                        }
                        else
                        {
                            LaserGunProps.laserBeamRenderer.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
                        }

                    }

                }

                if (hit.Length <= 0)
                {
                    LaserGunProps.laserBeamRenderer.SetPosition(1, _spawnPoint.up * _turretData.laserGun.range);
                    LaserGunProps.endVFX.transform.position = LaserGunProps.laserBeamRenderer.GetPosition(1);
                }

                _currentAmmo -= _turretData.laserGun.ammoConsumeRate / Time.time;
                _currentAmmo = Mathf.Clamp(_currentAmmo, 0f, _MaxAmmo);
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
            LaserGunProps.audioSourceClips.clip = _turretData.laserGun.lasergunFire;
            _hitVFX = LaserGunProps.hitVFX.GetComponent<ParticleSystem>();
        }

        private void InitParticlesList()
        {
            for (int i = 0; i < LaserGunProps.startVFX.transform.childCount; ++i)
            {
                var ps = LaserGunProps.startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps)
                {
                    _particles.Add(ps);
                }
            }

            for (int i = 0; i < LaserGunProps.endVFX.transform.childCount; ++i)
            {
                var ps = LaserGunProps.endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (ps)
                {
                    _particles.Add(ps);
                }
            }

        }

        private void EnableLaser()
        {
            LaserGunProps.laserBeamRenderer.enabled = true;
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i].Play();
            }

            LaserGunProps.audioSourceClips.PlayOneShot(_turretData.laserGun.lasergunFire);
            LaserGunProps.audioSourceClips.clip = _turretData.laserGun.lasergunBeam;
            LaserGunProps.audioSourceClips.Play();
        }

        public void DisableLaser()
        {
            LaserGunProps.laserBeamRenderer.enabled = false;
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i].Stop();
            }
            LaserGunProps.audioSourceClips.Stop();
        }

    }

}