using System;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.UI;

public class TurretCannon : MonoBehaviour
{
    [Header("Properties")]
    public Transform CannonHandler;
    public Transform CannonFirePoint;

    public Text AmmoText;
    public float CannonHandlerSpeed = 10.0f;

    [Header("Exposed variables")]
    public float repairHealth;
    //public int mCurrentAmmo;

    //[Header("Bullets factors")]
    //public float spreadFactor = 0.1f;
    //public float FireRate = 100f;
    //private float mTimeToFire = 0f;
    //public int AmmoMax = 10;
    //private ObjectPooler mObjectPooler;

    private InputReciever mInputReciever;
    private TurretHealth mTurretHealth;
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

    public DispenserData.Type _ammoType = DispenserData.Type.Normal;

    #region Weapons set

    private WeaponNormalGun _weaponNormalGun;
    private WeaponLaserBeam _weaponLaserBeam;

    #endregion

    void Start()
    {
        _ = TryGetComponent(out mInputReciever);
        _ = TryGetComponent(out _weaponNormalGun);
        _ = TryGetComponent(out _weaponLaserBeam);

        AmmoText.text = _weaponNormalGun.CurrentAmmo.ToString();
        mTurretHealth = GetComponentInParent<TurretHealth>();
        _laserSight = transform.parent.GetComponentInChildren<LineRenderer>();

        if (TryGetComponent<TurretLoader>(out var turretLoader))
        {
            turretLoader.OnReloadTurret += (_ammoType) => Reload(_ammoType);
        }
    }

    private void Update()
    {
        HandlerCannon();
    }

    private void HandlerCannon()
    {
        if (mTurretHealth.IsAlive)
        {
            CannonHandler.transform.Rotate(0.0f, 0.0f, -mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed * Time.deltaTime);
            bool setFire = mInputReciever.GetSecondaryHoldInput();

            switch (_ammoType)
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
        _laserSight.gameObject.SetActive(mInputReciever.GetInUse() && _ammoType != DispenserData.Type.LaserBeam);

        //if (mTurretHealth.IsAlive())
        //{
        //    CannonHandler.transform.Rotate(0.0f, 0.0f, -mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed * Time.deltaTime);
        //    Fire(mInputReciever.GetSecondaryHoldInput());
        //}

        //mLineRenderer.gameObject.SetActive(mInputReciever.GetInUse());
    }

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
                    IDamageable<float> damageable = collider.GetComponentInParent<BasicEnemy>();
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
        Debug.Log($"[{gameObject.transform.parent.name}] Turret reloaded!");
        _ammoType = type;
        switch (_ammoType)
        {
            case DispenserData.Type.Normal:
                {
                    _weaponNormalGun.Reload();
                }
                break;
            case DispenserData.Type.LaserBeam:
                LaserAmmo = 100;
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