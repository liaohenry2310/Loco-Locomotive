using System;
using UnityEngine;
using UnityEngine.UI;

public class TurretCannon : MonoBehaviour
{
    [Header("Properties")]
    public GameObject BulletsPrefabs;
    public Transform CannonHandler;
    public Transform CannonFirePoint;
    public Text AmmoCountText;
    public Text TurretText;
    public Text TurretRepairText;
    public float FireRate = 100f;
    public float CannonHandlerSpeed = 10.0f;
    public int AmmoMax = 10;

    //public int ammo;
    public float repairHealth;

    public int mCurrentAmmo;
    //public float turretMaxHealth;
    //private float mTurretHealth;

    private InputReciever mInputReciever;
    private float mTimeToFire = 0f;

    private TurretHealth mTurretHealth;

    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
        mCurrentAmmo = AmmoMax;
        //AmmoCountText.text = $"Ammo: {mCurrentAmmo}";
        //mTurretHealth = turretMaxHealth;
        mTurretHealth = FindObjectOfType<TurretHealth>();

    }

    private void Update()
    {
        CannonHandler.transform.Rotate(0.0f, 0.0f, mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed);
        Fire(mInputReciever.GetSecondaryHoldInput());
    }

    public void Fire(bool setFire)
    {
        if ((!mTurretHealth.IsDestroyed) && setFire && (mCurrentAmmo > 0) && (Time.time >= mTimeToFire))
        {
            mTimeToFire = Time.time + (1f / FireRate);
            var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
            mCurrentAmmo--;
            x.transform.rotation = CannonFirePoint.rotation;
            //AmmoCountText.text = $"Ammo: {--mCurrentAmmo}";
        }
        if (mCurrentAmmo == 0)
        {
            //AmmoCountText.text = $"Ammo ...... Run out ammo........!!";
        }
    }

    //public void TakeDamage(float takingDamage)
    //{
    //    mTurretHealth -= takingDamage;
    //    Debug.Log($"{ mTurretHealth }");
    //    //TurretText.text = $"! Warning !   Turret taking {takingDamage} damage ! Turret Health: {mTurretHealth}";
    //    if (mTurretHealth <= 0.0f)
    //    {
    //        Debug.Log("See you next time!");
    //        //TurretText.text = $"! Turret is Out Of Order ! ";
    //    }

    //}

    //public void Repair()
    //{
    //    Debug.Log($"{gameObject.transform.parent.name} Turret repaired!");
    //    mTurretHealth += repairHealth;
    //    //TurretRepairText.text = $"Turret repairing: {repairHealth} , Turret Health: {mTurretHealth}";
    //    if (mTurretHealth >= turretMaxHealth)
    //    {
    //        mTurretHealth = turretMaxHealth;
    //        //TurretRepairText.text = $"Turret health reached Maximum ";
    //    }
    //}

    public void Reload()
    {
        Debug.Log($"{gameObject.transform.parent.name} Turret reloaded!");
        mCurrentAmmo = AmmoMax;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretCannon = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.turretCannon = null;
        }
    }

}