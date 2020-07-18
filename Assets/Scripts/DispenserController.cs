using System;
using System.Collections.Generic;
using UnityEngine;

public class DispenserController : MonoBehaviour
{
    public event Action<DispenserAmmoType> OnGetDispenser;

    [SerializeField]
    private SpriteRenderer mSpriteDispenser = default;

    public Dictionary<DispenserAmmoType, Color> GetDictinoary { get; } = new Dictionary<DispenserAmmoType, Color>(5)
    {
        { DispenserAmmoType.None, Color.white},
        { DispenserAmmoType.Normal, Color.green},
        { DispenserAmmoType.LaserBean, Color.red},
        { DispenserAmmoType.Missile, Color.green},
        { DispenserAmmoType.Railgun, Color.blue}
    };

    public void GetDispenser(DispenserAmmoType type)
    {
        OnGetDispenser?.Invoke(type);
    }


}
