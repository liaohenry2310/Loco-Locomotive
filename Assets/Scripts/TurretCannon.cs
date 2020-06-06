using UnityEngine;
using UnityEngine.UI;

public class TurretCannon : MonoBehaviour
{
    [Header("Properties")]
    public GameObject BulletsPrefabs;
    public Transform CannonHandler;
    public Transform CannonFirePoint;

    public Text AmmoText;
    public float FireRate = 100f;
    public float CannonHandlerSpeed = 10.0f;
    public int AmmoMax = 10;
    public int mCurrentAmmo;

    private InputReciever mInputReciever;
    private float mTimeToFire = 0f;

    private TurretHealth mTurretHealth;

    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
        mCurrentAmmo = AmmoMax;
        mTurretHealth = FindObjectOfType<TurretHealth>();
        AmmoText.text = "Ammo: " + mCurrentAmmo.ToString();
    }

    private void Update()
    {
        if ((!mTurretHealth.IsDestroyed))
        {
            CannonHandler.transform.Rotate(0.0f, 0.0f, mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed);
            Fire(mInputReciever.GetSecondaryHoldInput());
        }
    }

    public void Fire(bool setFire)
    {
        if (setFire && (mCurrentAmmo > 0) && (Time.time >= mTimeToFire))
        {
            mTimeToFire = Time.time + (1f / FireRate);
            var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
            mCurrentAmmo--;
            x.transform.rotation = CannonFirePoint.rotation;


        }
        if (mCurrentAmmo == 0)
        {
            //AmmoCountText.text = $"Ammo ...... Run out ammo........!!";
        }
    }

    //public void Repair()
    //{
    //    mTurretHealth.RepairTurret(repairHealth);
    //}

    public void Reload()
    {
        Debug.Log($"{gameObject.transform.parent.name} Turret reloaded!");
        mCurrentAmmo = AmmoMax;
    }



}