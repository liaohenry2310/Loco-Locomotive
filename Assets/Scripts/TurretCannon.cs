using UnityEngine;
using UnityEngine.UI;

public class TurretCannon : MonoBehaviour
{
    [Header("Turret Properties")]
    public Transform CannonHandler;
    public Transform CannonFirePoint;

    public Text AmmoText;
    public float CannonHandlerSpeed = 10.0f;

    [Header("Exposed variables")]
    public float repairHealth;

    private InputReciever _inputReciever;
    private TurretHealth _turretHealth;
    private LineRenderer _laserSight;

    [Header("Laser Ammo set")]
    public LineRenderer LaserBeamer;
    public float LaserRange = 50f;
    public float LaserDamage = 0.5f;
    public float LaserAmmo = 100f;

    [Header("Railgun Ammo set")]
    public float RailgunRange = 50f;
    public float RailgunDamage = 20f;
    public int RailgunAmmo = 100;
    public float RailgunTimeFireRate = 0f;
    public float RailgunFireRate = 10f;

    public DispenserData.Type ammoType = DispenserData.Type.Normal;

    private bool _isPlayerUsingGamePad = false;

    #region Weapons set

    private WeaponNormalGun _weaponNormalGun;
    private WeaponLaserBeam _weaponLaserBeam;

    #endregion

    void Start()
    {
        _ = TryGetComponent(out _inputReciever);
        _ = TryGetComponent(out _weaponNormalGun);
        _ = TryGetComponent(out _weaponLaserBeam);

        AmmoText.text = _weaponNormalGun.CurrentAmmo.ToString();
        _turretHealth = GetComponentInParent<TurretHealth>();
        _laserSight = transform.parent.GetComponentInChildren<LineRenderer>();

        if (TryGetComponent<TurretLoader>(out var turretLoader))
        {
            turretLoader.OnReloadTurret += (_ammoType) => Reload(_ammoType);
        }

        _isPlayerUsingGamePad = _inputReciever.IsUsingGamepad();
    }

    private void Update()
    {
        HandlerCannon();
    }

    private void HandlerCannon()
    {
        if (_turretHealth.IsAlive)
        {
            // Setting here either player using or not the gamepad and change the current direction 
            float directionalX = _isPlayerUsingGamePad ?
                _inputReciever.GetDirectionalInput().x :
                _inputReciever.GetDirectionalInput().x;

            CannonHandler.transform.Rotate(0.0f, 0.0f, -directionalX * CannonHandlerSpeed * Time.deltaTime);
            bool setFire = _inputReciever.GetSecondaryHoldInput();

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
                        // Calling setFire from Weapon Laser Beam
                        _weaponLaserBeam.SetFire(setFire);
                        // Update the UI Text Canvas
                        AmmoText.text = _weaponLaserBeam.CurrentAmmo.ToString();
                    }
                    break;
                case DispenserData.Type.Missile:
                    break;
                case DispenserData.Type.Railgun:
                    {
                        //TODO: Testing area
                        Railgun(setFire);
                    }
                    break;
                case DispenserData.Type.RepairKit:
                case DispenserData.Type.Fuel:
                case DispenserData.Type.None:
                default:
                    break;
            }

        }
        _laserSight.gameObject.SetActive(IsUsingLaserSight);

        //if (mTurretHealth.IsAlive())
        //{
        //    CannonHandler.transform.Rotate(0.0f, 0.0f, -mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed * Time.deltaTime);
        //    Fire(mInputReciever.GetSecondaryHoldInput());
        //}

        //mLineRenderer.gameObject.SetActive(mInputReciever.GetInUse());
    }

    /// <summary>
    /// Allow to check if the Player is controlling and not using the laser beam to active the laser sight
    /// </summary>
    private bool IsUsingLaserSight => _inputReciever.GetInUse() && ammoType != DispenserData.Type.LaserBeam;

    private void Railgun(bool setFire)
    {
        if (setFire && LaserAmmo > 0f && (Time.time >= RailgunTimeFireRate))
        {
            RailgunTimeFireRate = Time.time + (1f / RailgunFireRate);
            if (!LaserBeamer.enabled)
            {
                LaserBeamer.enabled = true;
            }
            RaycastHit2D hit = Physics2D.Raycast(CannonFirePoint.transform.position, CannonFirePoint.transform.up, LaserRange);
            LaserBeamer.SetPosition(0, CannonFirePoint.transform.position);
            if (hit)
            {
                //LaserBeamer.SetPosition(1, LaserRange);
                Collider2D collider = hit.collider;
                if (collider)
                {
                    IDamageable<float> damageable = collider.GetComponentInParent<EnemyHealth>();
                    if (damageable != null)
                    {
                        //   damageable.TakeDamage(RailgunDamage);
                    }
                }
            }
            LaserBeamer.SetPosition(1, CannonFirePoint.transform.up * LaserRange);
            RailgunAmmo--;
        }
        else
        {
            LaserBeamer.enabled = false;
        }
    }

    public void Reload(DispenserData.Type type)
    {
        ammoType = type;
        switch (ammoType)
        {
            case DispenserData.Type.Normal:
                {
                    _weaponNormalGun.Reload();
                    Debug.Log($"[{gameObject.transform.parent.name}] Turret reloaded!");
                }
                break;
            case DispenserData.Type.LaserBeam:
                {
                    _weaponLaserBeam.Reload();
                    Debug.Log($"[{gameObject.transform.parent.name}] Turret reloaded!");
                }
                break;
            case DispenserData.Type.Missile:
                break;
            case DispenserData.Type.Railgun:
                break;
            default:
                break;
        }

        //mCurrentAmmo = AmmoMax;
    }
}