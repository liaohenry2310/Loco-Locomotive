using Interfaces;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Turret
{
    public class TurretGuns : MonoBehaviour, IInteractable
    {
        [Header("Data")]
        [SerializeField] private TurretData _turretData = null;

        [Header("Generals")]
        [SerializeField] private Transform _cannonHandler = null;
        [SerializeField] private Transform _spawnPointFire = null;

        [Header("Laser")]
        [SerializeField] private LineRenderer _LaserBeam = null;

        private PlayerV1 _player = null;
        private Weapons _weapons = null;
        private Vector2 _rotation = Vector2.zero;
        private bool _holdFire = false;

        private void Awake()
        {
            // Initialize with Machine Gun as default
            _weapons = new MachineGun(_turretData);
            _weapons.SetUp(_spawnPointFire);
        }

        private void FixedUpdate()
        {
            //if (!_turretHealth.IsAlive) return;

            float rotationSpeed = -_rotation.x * _turretData.AimSpeed * Time.fixedDeltaTime;
            if (_holdFire)
            {
                _weapons.SetFire();
                if (_weapons as LaserBeam != null)
                {
                    rotationSpeed *= _turretData.laserGun.aimSpeedMultiplier;
                }
            }
            else
            {
                // disable Line Renderer when using LaserBeam
                _LaserBeam.enabled = false;
            }
            _cannonHandler.Rotate(0f, 0f, rotationSpeed);
        }

        public void Interact(PlayerV1 player)
        {
            _player = player;
            _player.Interactable = this;
            _player.SwapActionControlToPlayer(false);

            Item item = _player.GetItem;
            if (item)
            {
                Reload(item.ItemType);
                item.DestroyAfterUse();
            }
        }

        #region Turret Action given to Player

        public void OnRotate(InputAction.CallbackContext context)
        {
            _rotation = context.ReadValue<Vector2>();
        }

        public void OnDetach(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _player.SwapActionControlToPlayer(true);
                _player.Interactable = null;
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            _holdFire = context.ReadValue<float>() >= 0.9f;
        }

        #endregion

        private void Reload(DispenserData.Type itemType)
        {
            switch (itemType)
            {
                case DispenserData.Type.Normal:
                    _weapons = new MachineGun(_turretData);
                    _weapons.SetUp(_spawnPointFire);
                    break;
                case DispenserData.Type.LaserBeam:
                    _weapons = new LaserBeam(_turretData);
                    _weapons.SetUp(_spawnPointFire, _LaserBeam);
                    break;
                case DispenserData.Type.Missile:
                    _weapons = new MissileGun(_turretData);
                    _weapons.SetUp(_spawnPointFire);
                    break;
                default:
                    break;
            }
            _weapons.Reload();
        }

    }

}