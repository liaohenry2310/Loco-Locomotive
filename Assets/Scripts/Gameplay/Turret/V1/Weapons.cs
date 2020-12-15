using UnityEngine;

namespace Turret
{
    public abstract class Weapons
    {
        protected Transform _spawnPoint = null;
        protected float _currentAmmo = 0.0f;
        protected float _MaxAmmo = 0.0f;

        public abstract void SetUp(Transform spawnPoint);

        public abstract void SetFire(bool fire);

        public virtual void Reload() => _currentAmmo = _MaxAmmo;

        public virtual float CurretAmmo => _currentAmmo;

        public virtual float MaxAmmo => _MaxAmmo;

    }

}

