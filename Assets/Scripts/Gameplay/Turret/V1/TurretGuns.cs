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

        [Header("MachineGun")]
        [SerializeField] private GameObject _MachineGunStartVFX = null;

        [Header("Laser")]
        [SerializeField] private LineRenderer _LaserBeam = null;
        [SerializeField] private GameObject _LaserBeamStartVFX = null;
        [SerializeField] private GameObject _LaserBeamEndVFX = null;

        private PlayerV1 _player = null;
        private Weapons _weapons = null;
        private LaserBeam.LaserVFXProperties _laserVFX;
        private MachineGun.MachineGunVFXProperties _machineGunVFX;

        private Vector2 _rotation = Vector2.zero;
        private bool _holdFire = false;

        private void Awake()
        {
            // Setting up laser properties
            _laserVFX.laserBeamRenderer = _LaserBeam;
            _laserVFX.startVFX = _LaserBeamStartVFX;
            _laserVFX.endVFX = _LaserBeamEndVFX;

            _machineGunVFX.muzzleFlashVFX = _MachineGunStartVFX;

            // Initialize with Machine Gun as default
            // Setting up Machine Gun properties
            //_weapons = new MachineGun(_turretData);
            //if (_weapons is MachineGun machineGun)
            //{
            //    machineGun.MachineGunVFX = _machineGunVFX;
            //}
            //_weapons.SetUp(_spawnPointFire);

            _weapons = new EmpGun(_turretData);
            _weapons.SetUp(_spawnPointFire);
           
        }

        private void FixedUpdate()
        {
            //if (!_turretHealth.IsAlive) return;
            float rotationSpeed = -_rotation.x * _turretData.AimSpeed * Time.fixedDeltaTime;
            _weapons.SetFire(_holdFire);
            if (_holdFire)
            {
                if (_weapons as LaserBeam != null)
                {
                    rotationSpeed *= _turretData.laserGun.aimSpeedMultiplier;
                }

                if (_weapons as EmpGun != null)
                {
                    rotationSpeed *= _turretData.empShockWave.aimSpeedMultiplier;
                }
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
                    if (_weapons is MachineGun machineGun)
                    {
                        machineGun.MachineGunVFX = _machineGunVFX;
                    }
                    _weapons.SetUp(_spawnPointFire);
                    _weapons.Reload();
                    break;
                case DispenserData.Type.LaserBeam:
                    _weapons = new LaserBeam(_turretData);
                    if (_weapons is LaserBeam laserbeam)
                    {
                        laserbeam.LaserVFX = _laserVFX;
                    }
                    _weapons.SetUp(_spawnPointFire);
                    _weapons.Reload();
                    break;
                case DispenserData.Type.Missile:
                    _weapons = new MissileGun(_turretData);
                    _weapons.SetUp(_spawnPointFire);
                    _weapons.Reload();
                    break;
                case DispenserData.Type.EMP:
                    _weapons = new EmpGun(_turretData);
                    _weapons.SetUp(_spawnPointFire);
                    _weapons.Reload();
                    break;
                case DispenserData.Type.Shield:
                    Debug.Log("[TurretGun] -- Shield is not implement yet.");
                    break;
                default:
                    break;
            }
        }

    }

}