using UnityEngine;

namespace Turret
{
    public abstract class Weapons
    {
        protected Transform _spawnPoint = null;
        protected float _currentAmmo = 0.0f;
        protected AudioClip _audioClip = null;
        protected SpriteRenderer _sprite = null;

        public abstract void SetUp(Transform spawnPoint);

        public abstract void SetFire(bool fire);

        public abstract void Reload();

        public abstract float CurretAmmo();


    }

}

