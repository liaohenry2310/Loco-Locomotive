using Interfaces;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ---- LEGACY CODE -----
/// Should be deleted in the FUTURE.
/// Cyro.
/// </summary>
public class TurretCannon : MonoBehaviour, IInteractable
{
    [Header("Turret Publics Attr")]
    [SerializeField] private Transform _cannonHandler = null;
    [SerializeField] private Text AmmoText = null;
    [SerializeField] private DispenserData.Type ammoType = DispenserData.Type.Normal;
    [SerializeField] private float CannonHandlerSpeed = 55.0f;

    //TODO: Refactor
    //private InputReciever _inputReciever;
    private TurretHealth _turretHealth;
    private LineRenderer _laserSight;

    private PlayerV1 _player;
    private Vector2 _rotation;
    private bool _holdFire = false;

    #region Weapons set

    private WeaponNormalGun _weaponNormalGun;
    private WeaponLaserBeam _weaponLaserBeam;
    private WeaponMissile _weaponMissile;

    #endregion

    private void Awake()
    {
        //_ = TryGetComponent(out _inputReciever);
        _ = TryGetComponent(out _weaponNormalGun);
        _ = TryGetComponent(out _weaponMissile);
        _ = TryGetComponent(out _weaponLaserBeam);

        _turretHealth = GetComponentInParent<TurretHealth>();
        _laserSight = transform.parent.GetComponentInChildren<LineRenderer>();
        _laserSight.gameObject.SetActive(false);  // Disable by default for now
        //if (TryGetComponent<TurretLoader>(out var turretLoader))
        //{
        //    turretLoader.OnReloadTurret += (_ammoType) => Reload(_ammoType);
        //}
    }

    void Start()
    {
        AmmoText.text = $"{_weaponNormalGun.CurrentAmmo}";
    }

    private void FixedUpdate()
    {
        //if (!_turretHealth.IsAlive) return;
        //HandlerCannon();
        _cannonHandler.Rotate(0f, 0f, -_rotation.x);

        if (_holdFire)
        {
            HandlerCannon(true);
        }
    }

    //TODO: Refactoring
    /// <summary>
    ///  Logic to use proper the analogic left sitck from Gamepad.
    ///  Reference:
    /// https://web.archive.org/web/20130418234531/http://www.gamasutra.com/blogs/JoshSutphin/20130416/190541/Doing_Thumbstick_Dead_Zones_Right.php"
    /// </summary>
    //private void UsingGamePad(float deltaTime)
    //{
    //    const float deadzone = 0.25f;
    //    Vector2 stickInput = new Vector2(_inputReciever.GetHorizontalInput(), _inputReciever.GetVerticalInput());
    //    stickInput = (stickInput.magnitude < deadzone) ? Vector2.zero : stickInput.normalized; // * ((stickInput.magnitude - deadzone) / (1f - deadzone));
    //    float aimAngle = Mathf.Atan2(-stickInput.x, stickInput.y) * Mathf.Rad2Deg;
    //    Quaternion aimRotation = Quaternion.AngleAxis(aimAngle, _cannonHandler.transform.forward);

    //    //float timeSpeedSlerp = (stickInput.magnitude * CannonHandlerSpeed * deltaTime) * 0.010f;
    //    float timeSpeedSlerp = (stickInput.magnitude * CannonHandlerSpeed * deltaTime);
    //    //_cannonHandler.transform.rotation = Quaternion.Slerp(_cannonHandler.transform.rotation, aimRotation, timeSpeedSlerp);
    //    _cannonHandler.transform.rotation = Quaternion.RotateTowards(_cannonHandler.transform.rotation, aimRotation, timeSpeedSlerp);
    //}

    private void HandlerCannon(bool setFire)
    {
        // Setting here either player using or not the gamepad and change the current direction 
        float time = Time.fixedDeltaTime;
        //TODO: Refactoring
        //bool setFire = _inputReciever.GetSecondaryHoldInput();

        switch (ammoType)
        {
            case DispenserData.Type.Normal:
                {
                    // Calling setFire from Weapon Normal
                    _weaponNormalGun.SetFire(setFire);
                    // Update the UI Text Canvas
                    AmmoText.text = _weaponNormalGun.CurrentAmmo.ToString();
                }
                break;
            case DispenserData.Type.LaserBeam:
                {
                    // only condition to use laser beam cannon time * 0.5f
                    time *= 0.5f;
                    // Calling setFire from Weapon Laser Beam
                    _weaponLaserBeam.SetFire(setFire, _turretHealth.IsAlive);
                    // Update the UI Text Canvas
                    AmmoText.text = _weaponLaserBeam.CurrentAmmo.ToString();
                }
                break;
            case DispenserData.Type.Missile:
                {
                    // Calling setFire from Weapon Missile
                    _weaponMissile.SetFire(setFire);
                    // Update the UI Text Canvas
                    AmmoText.text = $"{_weaponMissile.CurrentAmmo}";
                }
                break;
            case DispenserData.Type.RepairKit:
            case DispenserData.Type.Fuel:
            case DispenserData.Type.None:
            default:
                break;
        }
        // Disable by default for now
        //_laserSight.gameObject.SetActive(IsUsingLaserSight); 

        //if (_inputReciever.IsUsingGamepad)
        //{
        //    UsingGamePad(time);
        //}
        //else
        //{
        //    float finalSpeed = -_inputReciever.DirectionalInput.x * CannonHandlerSpeed * time;
        //    _cannonHandler.transform.Rotate(0.0f, 0.0f, finalSpeed);
        //}
    }

    /// <summary>
    /// Allow to check if the Player is controlling and not using the laser beam to active the laser sight
    /// </summary>
   // private bool IsUsingLaserSight => _inputReciever.GetInUse() && ammoType != DispenserData.Type.LaserBeam;

    #region Railgun - BACKLOG

    //[Header("Railgun Ammo set")]
    //public float RailgunRange = 50f;
    //public float RailgunDamage = 20f;
    //public int RailgunAmmo = 100;
    //public float RailgunTimeFireRate = 0f;
    //public float RailgunFireRate = 10f;

    //private void Railgun(bool setFire)
    //{
    //    if (setFire && LaserAmmo > 0f && (Time.time >= RailgunTimeFireRate))
    //    {
    //        RailgunTimeFireRate = Time.time + (1f / RailgunFireRate);
    //        if (!LaserBeamer.enabled)
    //        {
    //            LaserBeamer.enabled = true;
    //        }
    //        RaycastHit2D hit = Physics2D.Raycast(CannonFirePoint.transform.position, CannonFirePoint.transform.up, LaserRange);
    //        LaserBeamer.SetPosition(0, CannonFirePoint.transform.position);
    //        if (hit)
    //        {
    //            //LaserBeamer.SetPosition(1, LaserRange);
    //            Collider2D collider = hit.collider;
    //            if (collider)
    //            {
    //                IDamageable<float> damageable = collider.GetComponentInParent<EnemyHealth>();
    //                if (damageable != null)
    //                {
    //                    //   damageable.TakeDamage(RailgunDamage);
    //                }
    //            }
    //        }
    //        LaserBeamer.SetPosition(1, CannonFirePoint.transform.up * LaserRange);
    //        RailgunAmmo--;
    //    }
    //    else
    //    {
    //        LaserBeamer.enabled = false;
    //    }
    //}

    #endregion

    public void Reload(DispenserData.Type type)
    {
        ammoType = type;
        switch (ammoType)
        {
            case DispenserData.Type.Normal:
                {
                    _weaponNormalGun.Reload();
                }
                break;
            case DispenserData.Type.LaserBeam:
                {
                    _weaponLaserBeam.Reload();
                }
                break;
            case DispenserData.Type.Missile:
                {
                    _weaponMissile.Reload();
                }
                break;
            default: break;
        }
        Debug.Log($"Ammo: {ammoType} reloaded!");
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

}