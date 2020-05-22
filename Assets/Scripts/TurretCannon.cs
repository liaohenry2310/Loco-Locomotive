using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class TurretCannon : MonoBehaviour
{
    [Header("Properties")]
    public GameObject Bullets;
    public Transform CannonHandler;
    public Transform CannonFirePoint;
    public Text AmmoCountText;
    public Text TurretText;
    public Text TurretRepairText;
    public float FireRate = 100f;
    public float CannonHandlerSpeed = 10.0f;
    public int AmmoMax = 10;
    private int mAmmo;
    public float turretMaxHealth;
    private float mTurretHealth;
    private bool mRunOutHealth;



    private InputReciever mInputReciever;
    private float mTimeToFire = 0f;

    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
        AmmoCountText.text = $"Ammo: {mAmmo}";
        mRunOutHealth = false;
        mTurretHealth = turretMaxHealth;
        mAmmo = AmmoMax;
    }

    void Update()
    {
        CannonHandler.transform.Rotate(0.0f, 0.0f, mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed);
        Fire(mInputReciever.GetSecondaryHoldInput());
    }

    public void Fire(bool setFire)
    {
        if ( mRunOutHealth ==false && setFire && (mAmmo > 0) && (Time.time >= mTimeToFire))
        {
            mTimeToFire = Time.time + (1f / FireRate);
            var x = Instantiate(Bullets, CannonFirePoint.transform.position, Quaternion.identity);
            x.transform.rotation = CannonFirePoint.rotation;
            AmmoCountText.text = $"Ammo: {--mAmmo}";
        }
        if (mAmmo==0)
        {
            AmmoCountText.text = $"Ammo ...... Run out ammo........!!";
        }
    }

    public void TakeDamage(float takingDamage)
    {
        mTurretHealth -= takingDamage;
        TurretText.text = $"! Warning !   Turret taking {takingDamage} damage ! Turret Health: {mTurretHealth}";
        if (mTurretHealth <= 0.0f)
        {
            TurretText.text = $"! Turret is Out Of Order ! ";
        }

    }

    public void Repair(float repairHealth)
    {
        mTurretHealth += repairHealth;
        TurretRepairText.text = $"Turret repairing: {repairHealth} , Turret Health: {mTurretHealth}";
        if (mTurretHealth>=turretMaxHealth)
        {
            mTurretHealth = turretMaxHealth;
            TurretRepairText.text = $"Turret health reached Maximum ";
        }
    }

    public void Reload(int ammo)
    {
        mAmmo += ammo;
        AmmoCountText.text = $"Ammo reloading: ... Current Ammo: {mAmmo}";
        if (mAmmo >= AmmoMax)
        {
            mAmmo = AmmoMax;
            AmmoCountText.text = $"Can not Reload Anymore ... Current Ammo: {mAmmo}";
        }
    }

    public void IsAlive()
    {
        if (mTurretHealth <= 0.0f)
        {
            mRunOutHealth = true;
        }
        else
        {
            mRunOutHealth = false;
        }
    }


}
