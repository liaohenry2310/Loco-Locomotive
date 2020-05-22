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
    public float FireRate = 100f;
    public float CannonHandlerSpeed = 10.0f;
    public int Ammo = 10;

    private InputReciever mInputReciever;
    private float mTimeToFire = 0f;

    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
        AmmoCountText.text = $"Ammo: {Ammo}";
    }

    void Update()
    {
        CannonHandler.transform.Rotate(0.0f, 0.0f, mInputReciever.GetDirectionalInput().x * CannonHandlerSpeed);
        Fire(mInputReciever.GetSecondaryHoldInput());
    }

    public void Fire(bool setFire)
    {
        if (setFire && (Ammo > 0) && (Time.time >= mTimeToFire))
        {
            mTimeToFire = Time.time + (1f / FireRate);
            var x = Instantiate(Bullets, CannonFirePoint.transform.position, Quaternion.identity);
            x.transform.rotation = CannonFirePoint.rotation;
            AmmoCountText.text = $"Ammo: {--Ammo}";
        }
    }

}
