using UnityEngine;
using UnityEngine.EventSystems;

namespace Turret
{
    public abstract class Weapons
    {
        protected TurretData _turretData = null;
        protected Transform _spawnPoint = null;
        protected ObjectPoolManager _objectPoolManager = null;
        protected float _currentAmmo = 0.0f;

        public struct LaserProperties
        {
            public LineRenderer laserBeamRenderer;
            public GameObject startVFX;
            public GameObject endVFX;
        }

        public Weapons(TurretData setupData)
        {
            _turretData = setupData;
        }

        public abstract void SetUp(Transform spawnPoint);

        public abstract void SetUp(Transform spawnPoint, LaserProperties laserBeam);

        public abstract void SetFire();

        public abstract void Reload();
    }

}

