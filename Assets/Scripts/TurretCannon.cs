using UnityEngine;
using UnityEngine.UI;

public class TurretCannon : MonoBehaviour
{
    [Header("Properties")]
    public GameObject BulletsPrefabs;
    public Transform CannonHandler;
    public Transform CannonFirePoint;

    public Text AmmoText;
    public float CannonHandlerSpeed = 10.0f;

    [Header("Exposed variables")]
    public float repairHealth;
    public int mCurrentAmmo;

    [Header("Bullets factors")]
    public float spreadFactor = 0.1f;
    public float FireRate = 100f;

    private InputReciever mInputReciever;
    private float mTimeToFire = 0f;

    private TurretHealth mTurretHealth;
    //public int AmmoMax = 10;
    private ObjectPooler mObjectPooler;

    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
        mTurretHealth = GetComponentInParent<TurretHealth>();
        AmmoText.text = "Ammo: " + mCurrentAmmo.ToString();
        mObjectPooler = GetComponent<ObjectPooler>();
        mCurrentAmmo = mObjectPooler.AmountToPool;
    }

    private void Update()
    {
        if (!mTurretHealth.IsAlive())
        {
            Debug.Log("You cannot use this turret!");
        }

        if (mTurretHealth.IsAlive())
        {
            CannonHandler.transform.Rotate(0.0f, 0.0f, -mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed * Time.deltaTime);
            Fire(mInputReciever.GetSecondaryHoldInput());
        }
    }
    
    public void Fire(bool setFire)
    {
        if (setFire && (mCurrentAmmo > 0) && (Time.time >= mTimeToFire))
        {
            mTimeToFire = Time.time + (1f / FireRate);
            // bck original
            //var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
            //var x = Instantiate(BulletsPrefabs, CannonFirePoint.transform.position, Quaternion.identity);
            //mCurrentAmmo--;
            //x.transform.rotation = Quaternion.RotateTowards(CannonFirePoint.transform.rotation, Random.rotation, spreadFactor);
            // bck original
            //x.transform.rotation = CannonFirePoint.rotation;
            //AmmoCountText.text = $"Ammo: {--mCurrentAmmo}";

            // novo teste
            var bullet = mObjectPooler.GetPooledObject();
            if (bullet)
            {
                bullet.transform.position = CannonFirePoint.transform.position;
                bullet.transform.rotation = Quaternion.RotateTowards(CannonFirePoint.transform.rotation, Random.rotation, spreadFactor);
                bullet.SetActive(true);
                mCurrentAmmo--;
            }
        }
        if (mCurrentAmmo == 0)
        {
            //AmmoCountText.text = $"Ammo ...... Run out ammo........!!";
        }
        AmmoText.text = "Ammo: " + mCurrentAmmo.ToString();
    }

    //public void Repair()
    //{
    //    mTurretHealth.RepairTurret(repairHealth);
    //}

    public void Reload()
    {
        Debug.Log($"{gameObject.transform.parent.name} Turret reloaded!");
        //mCurrentAmmo = AmmoMax;
        mCurrentAmmo = mObjectPooler.AmountToPool;
    }
}