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

        [Header("Sprite")]
        [SerializeField] private SpriteRenderer _upperSprite = null;
        [SerializeField] private SpriteRenderer _cannonSprite = null;
        [SerializeField] private SpriteRenderer _bottomSprite = null;
        [SerializeField] private TurretBase _turretBase = null;

        private PlayerV1 _player = null;
        private Weapons _weapons = null;
        private LaserBeam.LaserVFXProperties _laserVFX;
        private MachineGun.MachineGunVFXProperties _machineGunVFX;
        private MissileGun.MissileGunSFXprops _missileGunSFXprops;

        private Vector2 _rotation = Vector2.zero;
        private bool _holdFire = false;

        private float curretHealth;

        #region AudioSource
        public AudioSource Audio;
        private bool playLasergunFire = false;
        private float timer;
        #endregion

        private void Awake()
        {
            // Setting up laser properties
            _laserVFX.laserBeamRenderer = _LaserBeam;
            _laserVFX.startVFX = _LaserBeamStartVFX;
            _laserVFX.endVFX = _LaserBeamEndVFX;

            _machineGunVFX.muzzleFlashVFX = _MachineGunStartVFX;
           
            // Initialize with Machine Gun as default
            _weapons = new MachineGun(_turretData);
            if (_weapons is MachineGun machineGun)
            {
                machineGun.MachineGunVFX = _machineGunVFX;
            }
            _weapons.SetUp(_spawnPointFire);

            #region AudioSource
            Audio = gameObject.AddComponent<AudioSource>();
            Audio.playOnAwake = false;
            Audio.volume = 0.1f;
            Audio.pitch = Random.Range(0.9f, 1.1f);
            #endregion

            _missileGunSFXprops.audioCLips = Audio;

        }

        private void OnEnable()
        {
            _turretBase = FindObjectOfType<TurretBase>();
        }

        private void Update()
        {
            curretHealth = _turretBase.HealthSystem.HealthPercentage;
            if (_weapons as LaserBeam != null)
            {
                if (curretHealth >= 0.75f)
                {
                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[0];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[0];
                    _bottomSprite.sprite = _turretData.laserGun.Bottomsprites[0];
                }
                else if (curretHealth >= 0.25f && curretHealth < 0.75f)
                {
                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[1];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[1];
                    _bottomSprite.sprite = _turretData.laserGun.Bottomsprites[0];
                }
                else if (curretHealth >0f && curretHealth < 0.25f)
                {
                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[2];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[2];
                    _bottomSprite.sprite = _turretData.laserGun.Bottomsprites[0];
                }
                else if (curretHealth <= 0)
                {
                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[3];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[3];
                    _bottomSprite.sprite = _turretData.laserGun.Bottomsprites[1];
                }
            }
            else if (_weapons as MachineGun != null)
            {
                if (curretHealth >= 0.75f)
                {
                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[0];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[0];
                    _bottomSprite.sprite = _turretData.machineGun.Bottomsprites[0];
                }
                else if (curretHealth >= 0.25f && curretHealth < 0.75f)
                {
                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[1];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[1];
                    _bottomSprite.sprite = _turretData.machineGun.Bottomsprites[0];

                }
                else if (curretHealth > 0f && curretHealth < 0.25f)
                {
                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[2];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[2];
                    _bottomSprite.sprite = _turretData.machineGun.Bottomsprites[0];

                }
                else if (curretHealth <= 0)
                {
                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[3];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[3];
                    _bottomSprite.sprite = _turretData.machineGun.Bottomsprites[1];
                }

            }
            else if (_weapons as MissileGun != null)
            {
                if (curretHealth >= 0.75f)
                {
                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[0];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[0];
                    _bottomSprite.sprite = _turretData.missileGun.Bottomsprites[0];
                }
                else if (curretHealth >= 0.25f && curretHealth < 0.75f)
                {
                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[1];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[1];
                    _bottomSprite.sprite = _turretData.missileGun.Bottomsprites[0];
                }
                else if (curretHealth > 0f && curretHealth < 0.25f)
                {
                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[2];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[2];
                    _bottomSprite.sprite = _turretData.missileGun.Bottomsprites[0];
                }
                else if (curretHealth <= 0)
                {
                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[3];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[3];
                    _bottomSprite.sprite = _turretData.missileGun.Bottomsprites[1];
                }

            }
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

                    timer += Time.fixedDeltaTime;
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
                    if (_weapons.CurretAmmo == 0.0f)
                    {
                        Audio.clip = null;
                    }
                }
                else if(_weapons as MachineGun !=null)
                {
                    Audio.clip = _turretData.machineGun.machinegunFire;
                    Audio.Play();
                    if (_weapons.CurretAmmo == 0.0f)
                    {
                        Audio.clip = null;
                    }
                }
               
            }
           
            #region AudioSource
            if (!_holdFire && _weapons as LaserBeam != null)
            {
                Audio.clip = null;
                playLasergunFire = false;
                timer = 0;
            }
            #endregion

            if (_weapons as EmpGun != null)
            {
                rotationSpeed *= _turretData.empShockWave.aimSpeedMultiplier;
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
                    if (_weapons is MissileGun missile)
                    {
                        missile.MissileGunProps = _missileGunSFXprops;
                    }
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