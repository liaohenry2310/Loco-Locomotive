using System;
using UnityEngine;

public class DispenserAmmo : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private DispenserAmmoType mAmmoType = DispenserAmmoType.Normal;

    public DispenserAmmoType GetAmmoType => mAmmoType;

    //public DispenserAmmoType GetAmmoType() { return mAmmoType; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            //player.ammoType = GetAmmoType;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            //player.ammoType = DispenserAmmoType.None;
        }
    }

}
