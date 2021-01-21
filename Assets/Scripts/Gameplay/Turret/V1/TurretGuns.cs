using Interfaces;
using Items;
using System.Collections;
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
        [SerializeField] private TurretAmmoIndicator _turretAmmoIndicator = null;
        [SerializeField] private ParticleSystem _smokeParticle = null;
        [SerializeField] [Range(0.0f, 10.0f)] private float _smokeMaxEmission = 10.0f;

        [Header("MachineGun")]
        [SerializeField] private GameObject _MachineGunStartVFX = null;

        [Header("Laser")]
        [SerializeField] private LineRenderer _LaserBeam = null;
        [SerializeField] private GameObject _LaserBeamStartVFX = null;
        [SerializeField] private GameObject _LaserBeamEndVFX = null;
        [SerializeField] private GameObject _LaserBeamHitVFX = null;

        [Header("Shield")]
        [SerializeField] private ShieldGunController _shieldGunController = null;

        [Header("Sprite")]
        [SerializeField] private SpriteRenderer _upperSprite = null;
        [SerializeField] private SpriteRenderer _cannonSprite = null;
        [SerializeField] private SpriteRenderer _bottomSprite = null;
        [SerializeField] private TurretBase _turretBase = null;

        [Header("Squishes Effect")]
        [SerializeField] private Transform _pivotTurretGun = null;
        [SerializeField] private Animator _animator = null;

        private AudioSource _audioSource = null;
        private PlayerV1 _player = null;
        private Weapons _weapons = null;
        private LaserBeam.LaserGunProperties _laserGunProps;
        private MachineGun.MachineGunProperties _machineGunProps;
        private MissileGun.MissileGunProperties _missileGunProps;
        private ShieldGun.ShieldGunProperties _shieldGunProps;

        private Vector2 _rotation = Vector2.zero;
        private bool _holdFire = false;
        private ParticleSystem.EmissionModule _emission;
        private Vector2 _recoildSmoothDampVelocity;

        private Vector3 _cannonOriginalPosition;


        private void Awake()
        {
            if (!TryGetComponent(out _audioSource))
            {
                Debug.LogWarning("Fail to load Audio Source component.");
            }
            _emission = _smokeParticle.emission;

            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _cannonOriginalPosition = _cannonHandler.localPosition;

            // Setting up laser properties
            _laserGunProps.laserBeamRenderer = _LaserBeam;
            _laserGunProps.startVFX = _LaserBeamStartVFX;
            _laserGunProps.endVFX = _LaserBeamEndVFX;
            _laserGunProps.hitVFX = _LaserBeamHitVFX;
            _laserGunProps.audioSourceClips = _audioSource;
            _laserGunProps.transformCannon = _cannonHandler;

            // Setting up missile properties
            _missileGunProps.audioSourceClips = _audioSource;
            _missileGunProps.transformCannon = _cannonHandler;

            // Setting up machine gun properties
            _machineGunProps.muzzleFlashVFX = _MachineGunStartVFX;
            _machineGunProps.audioSourceClips = _audioSource;
            _machineGunProps.transformCannon = _cannonHandler;

            // Setting up shield gun properties
            _shieldGunProps.shieldGunController = _shieldGunController;

            // Initialize with Machine Gun as default
            _weapons = new MachineGun(_turretData);
            if (_weapons is MachineGun machineGun)
            {
                machineGun.MachineGunProps = _machineGunProps;
            }
            _weapons.SetUp(_spawnPointFire);

            // TODO: Just for Testing
            //_weapons = new ShieldGun(_turretData);
            //if (_weapons is ShieldGun shield)
            //{
            //    shield.ShieldGunPros = _shieldGunProps;
            //}
            //_weapons.SetUp(_spawnPointFire);
        }

        private void Start()
        {
            _turretBase.OnTakeDamageUpdate += UpdateBottonTurret;
            _turretBase.OnRepairUpdate += UpdateTurretSprite;
        }

        private void OnDisable()
        {
            _turretBase.OnTakeDamageUpdate -= UpdateBottonTurret;
            _turretBase.OnRepairUpdate -= UpdateTurretSprite;
        }

        private void UpdateTurretSprite()
        {
            if (_weapons as LaserBeam != null)
            {
                _upperSprite.sprite = _turretData.laserGun.Uppersprites[0];
                _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[0];
            }
            else if (_weapons as MachineGun != null)
            {
                _upperSprite.sprite = _turretData.machineGun.Uppersprites[0];
                _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[0];
            }
            else if (_weapons as MissileGun != null)
            {
                _upperSprite.sprite = _turretData.missileGun.Uppersprites[0];
                _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[0];
            }
            _bottomSprite.sprite = _turretData.Bottomsprites[0];
            _turretAmmoIndicator.EnableIndicator(true);
            _smokeParticle.Stop();
        }

        private void UpdateBottonTurret(float healthPerc)
        {
            _turretAmmoIndicator.EnableIndicator(healthPerc >= 0.1f);

            if (_weapons as LaserBeam != null)
            {

                if (healthPerc >= 0.75f)
                {
                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[0];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[0];
                }
                else if (healthPerc >= 0.25f && healthPerc < 0.75f)
                {
                    _emission.rateOverTime = Mathf.RoundToInt(_smokeMaxEmission / 4);

                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[1];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[1];
                }
                else if (healthPerc > 0f && healthPerc < 0.25f)
                {
                    _emission.rateOverTime = Mathf.RoundToInt(_smokeMaxEmission / 2);

                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[2];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[2];
                }
                else
                {
                    _emission.rateOverTime = _smokeMaxEmission;

                    _upperSprite.sprite = _turretData.laserGun.Uppersprites[3];
                    _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[3];
                    _bottomSprite.sprite = _turretData.Bottomsprites[1];
                }

                if (!(healthPerc >= 0.75f) && _smokeParticle.isStopped)
                {
                    _smokeParticle.Play();
                }
            }

            if (_weapons as MachineGun != null)
            {
                if (healthPerc >= 0.75f)
                {
                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[0];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[0];
                }
                else if (healthPerc >= 0.25f && healthPerc < 0.75f)
                {
                    _emission.rateOverTime = Mathf.RoundToInt(_smokeMaxEmission / 4);

                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[1];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[1];
                }
                else if (healthPerc > 0f && healthPerc < 0.25f)
                {
                    _emission.rateOverTime = Mathf.RoundToInt(_smokeMaxEmission / 2);

                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[2];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[2];
                }
                else
                {
                    _emission.rateOverTime = _smokeMaxEmission;

                    _upperSprite.sprite = _turretData.machineGun.Uppersprites[3];
                    _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[3];
                    _bottomSprite.sprite = _turretData.Bottomsprites[1];
                }

                if (!(healthPerc >= 0.75f) && _smokeParticle.isStopped)
                {
                    _smokeParticle.Play();
                }

            }

            if (_weapons as MissileGun != null)
            {
                if (healthPerc >= 0.75f)
                {
                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[0];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[0];
                }
                else if (healthPerc >= 0.25f && healthPerc < 0.75f)
                {
                    _emission.rateOverTime = Mathf.RoundToInt(_smokeMaxEmission / 4);

                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[1];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[1];
                }
                else if (healthPerc > 0f && healthPerc < 0.25f)
                {
                    _emission.rateOverTime = Mathf.RoundToInt(_smokeMaxEmission / 2);

                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[2];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[2];
                }
                else
                {
                    _emission.rateOverTime = _smokeMaxEmission;

                    _upperSprite.sprite = _turretData.missileGun.Uppersprites[3];
                    _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[3];
                    _bottomSprite.sprite = _turretData.Bottomsprites[1];
                }

                if (!(healthPerc >= 0.75f) && _smokeParticle.isStopped)
                {
                    _smokeParticle.Play();
                }

            }
        }


        private void FixedUpdate()
        {
            // recoil effect to back to original position
            _machineGunProps.transformCannon.localPosition = Vector2.SmoothDamp(_machineGunProps.transformCannon.localPosition, _cannonOriginalPosition, ref _recoildSmoothDampVelocity, .1f);

            if (!_turretBase.HealthSystem.IsAlive)
            {
                if (_weapons is LaserBeam laser)
                {
                    laser.DisableLaser();
                }
                return;
            }

            float rotationSpeed = -_rotation.x * _turretData.AimSpeed * Time.fixedDeltaTime;
            _weapons.SetFire(_holdFire);
            _turretAmmoIndicator.UpadteIndicator(ref _weapons);
            if (_weapons as ShieldGun != null)
            {
                rotationSpeed = 0.0f;
            }

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

        public bool isInUse = false;

        public void Interact(PlayerV1 player)
        {
            //TODO: whicht time the player will be atached on the turret?
            _player = player;
            Item item = _player.GetItem;
            if (item)
            {
                _player.animator.SetBool("IsHoldItem", false);
                Reload(item.ItemType);
                item.DestroyAfterUse();
            }
            else
            {
                _player.Interactable = this;
                isInUse = true;
                _player.SwapActionControlToPlayer(false);
                _player.transform.position = this.transform.position;
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
                isInUse = false;
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            _holdFire = context.ReadValue<float>() >= 0.9f;
        }

        #endregion


        private readonly int _active = Animator.StringToHash("Active");
        private void Reload(DispenserData.Type itemType)
        {
            //_animator.SetTrigger(_active);
            switch (itemType)
            {
                case DispenserData.Type.Normal:
                    SetMachineGun();
                    break;
                case DispenserData.Type.LaserBeam:
                    SetLaserBeam();
                    break;
                case DispenserData.Type.Missile:
                    SetMissileGun();
                    break;
                case DispenserData.Type.EMP:
                    SetEMPGun();
                    break;
                case DispenserData.Type.Shield:
                    SetShieldGun();
                    break;
                default:
                    break;
            }

            //StartCoroutine(SquisheEffect());
        }

        private float easeOutElastic(float time)
        {
            const float c4 = (2 * Mathf.PI) / 3;

            return time == 0
              ? 0
              : time == 1
              ? 1
              : Mathf.Pow(2.0f, -10.0f * time) * Mathf.Sin((time * 10.0f - 0.75f) * c4) + 1.0f;
        }

        private IEnumerator SquisheEffect()
        {
            Transform originalPos = _pivotTurretGun.transform;
            float time = 0.0f;
            const float threshold = 0.5f;
            while (time <= threshold)
            {
                time += Time.deltaTime;
                //float interpolation = easeOutElastic(time);
                float lerpY = Mathf.Lerp(_pivotTurretGun.transform.localScale.y, 0.5f, time);
                _pivotTurretGun.transform.localScale -= new Vector3(0.0f, _pivotTurretGun.transform.localScale.y - lerpY, 0.0f);
                float lerpX = Mathf.Lerp(_pivotTurretGun.transform.localScale.x, 0.5f, time);
                _pivotTurretGun.transform.localScale += new Vector3(lerpX, 0.0f, 0.0f);

                if (lerpY == 0.5f)
                {
                }
                yield return null;

            }
            //_pivotTurretGun.transform.localPosition = originalPos.localPosition;
        }

        private void SetMachineGun()
        {
            _weapons = new MachineGun(_turretData);
            if (_weapons is MachineGun machineGun)
            {
                machineGun.MachineGunProps = _machineGunProps;
            }
            _weapons.SetUp(_spawnPointFire);
            _weapons.Reload();
            _cannonSprite.enabled = true;
            _upperSprite.sprite = _turretData.machineGun.Uppersprites[0];
            _cannonSprite.sprite = _turretData.machineGun.Cannonsprites[0];
        }

        private void SetLaserBeam()
        {
            _weapons = new LaserBeam(_turretData);
            if (_weapons is LaserBeam laserbeam)
            {
                laserbeam.LaserGunProps = _laserGunProps;
            }
            _weapons.SetUp(_spawnPointFire);
            _weapons.Reload();
            _cannonSprite.enabled = true;
            _upperSprite.sprite = _turretData.laserGun.Uppersprites[0];
            _cannonSprite.sprite = _turretData.laserGun.Cannonsprites[0];
        }

        private void SetMissileGun()
        {
            _weapons = new MissileGun(_turretData);
            if (_weapons is MissileGun missile)
            {
                missile.MissileGunProps = _missileGunProps;
            }
            _weapons.SetUp(_spawnPointFire);
            _weapons.Reload();
            _cannonSprite.enabled = true;
            _upperSprite.sprite = _turretData.missileGun.Uppersprites[0];
            _cannonSprite.sprite = _turretData.missileGun.Cannonsprites[0];
        }

        private void SetEMPGun()
        {
            _weapons = new EmpGun(_turretData);
            _weapons.SetUp(_spawnPointFire);
            _cannonSprite.enabled = true;
            _weapons.Reload();
        }

        private void SetShieldGun()
        {
            _weapons = new ShieldGun(_turretData);
            if (_weapons is ShieldGun shield)
            {
                shield.ShieldGunPros = _shieldGunProps;
            }
            _weapons.SetUp(_spawnPointFire);
            _weapons.Reload();
            _cannonSprite.enabled = false;
            _cannonHandler.rotation = Quaternion.identity;
        }
    }

}