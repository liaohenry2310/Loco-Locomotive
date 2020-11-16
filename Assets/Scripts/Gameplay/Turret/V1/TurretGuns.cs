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

        [Header("EMP")]
        [SerializeField] private GameObject _EMPGunPulseVFX = null;

        [Header("Laser")]
        [SerializeField] private LineRenderer _LaserBeam = null;
        [SerializeField] private GameObject _LaserBeamStartVFX = null;
        [SerializeField] private GameObject _LaserBeamEndVFX = null;

        private PlayerV1 _player = null;
        private Weapons _weapons = null;
        private LaserBeam.LaserVFXProperties _laserVFX;
        private MachineGun.MachineGunVFXProperties _machineGunVFX;
        private EmpGun.EMPGunVFXProperties _empGunPulse;

        private Vector2 _rotation = Vector2.zero;
        private bool _holdFire = false;

        #region AudioSource
        public AudioSource Audio;
        private bool playLasergunFire =false;
        private float timer;
        #endregion

        private void Awake()
        {
            // Setting up laser properties
            _laserVFX.laserBeamRenderer = _LaserBeam;
            _laserVFX.startVFX = _LaserBeamStartVFX;
            _laserVFX.endVFX = _LaserBeamEndVFX;

            _machineGunVFX.muzzleFlashVFX = _MachineGunStartVFX;
            _empGunPulse.muzzleFlashVFX = _EMPGunPulseVFX;

            // Initialize with Machine Gun as default
            // Setting up Machine Gun properties
            //_weapons = new MachineGun(_turretData);
            //if (_weapons is MachineGun machineGun)
            //{
            //    machineGun.MachineGunVFX = _machineGunVFX;
            //}
            //_weapons.SetUp(_spawnPointFire);

            _weapons = new EmpGun(_turretData);

            if (_weapons is EmpGun empGun)
            {
                empGun.EmpGunVFX = _empGunPulse;
            }
            _weapons.SetUp(_spawnPointFire);



            // Setting up laser properties
            _laserVFX.laserBeamRenderer = _LaserBeam;
            _laserVFX.startVFX = _LaserBeamStartVFX;
            _laserVFX.endVFX = _LaserBeamEndVFX;

           
            #region AudioSource
            Audio = gameObject.AddComponent<AudioSource>();
            Audio.playOnAwake = false;
            Audio.volume = 0.1f;
            Audio.pitch = Random.Range(0.9f, 1.1f);
            #endregion

        }

        private void FixedUpdate()
        {
            //if (!_turretHealth.IsAlive) return;
            float rotationSpeed = -_rotation.x * _turretData.AimSpeed * Time.fixedDeltaTime;
            _weapons.SetFire(_holdFire);
            if (_holdFire)

            {
                _weapons.SetFire();

                if (_weapons as LaserBeam != null)
                {
                    rotationSpeed *= _turretData.laserGun.aimSpeedMultiplier;
                    timer += Time.deltaTime;
                    if(playLasergunFire ==false)
                    {                       
                        Audio.clip = _turretData.laserGun.lasergunFire;
                        Audio.Play();
                        playLasergunFire = true;
                    }
                    if (timer >= 0.1f)
                    {
                        Audio.clip = _turretData.laserGun.lasergunBeam;
                        Audio.Play();
                    }                                         
                    if (_weapons.CurretAmmo() == 0.0f)
                    {
                        Audio.clip = null;
                    }
                }
                else if(_weapons as MachineGun !=null)
                {
                    Audio.clip = _turretData.machineGun.machinegunFire;
                    Audio.Play();
                    if (_weapons.CurretAmmo() == 0.0f)
                    {
                        Audio.clip = null;
                    }
                }
                else if(_weapons as MissileGun !=null)
                {
                    Audio.clip = _turretData.missileGun.missilegunFire;
                    Audio.Play();
                    if (_weapons.CurretAmmo() == 0.0f)
                    {
                        Audio.clip = null;
                    }
                }
                
                 if (_weapons as EmpGun != null)
                {
                    rotationSpeed *= _turretData.empShockWave.aimSpeedMultiplier;
                }
            }
            else
            {
                // disable Line Renderer when using LaserBeam
                _LaserBeam.enabled = false;
            }
            #region AudioSource
            if (!_holdFire && _weapons as LaserBeam != null)
            {
                Audio.clip = null;
                playLasergunFire = false;
                timer = 0;
            }
            #endregion

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
                _player.animator.SetBool("IsHoldItem", false);
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
                    if (_weapons is EmpGun empGun)
                    {
                        empGun.EmpGunVFX = _empGunPulse;
                    }
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