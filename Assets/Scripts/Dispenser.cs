using UnityEngine;
using System.Collections.Generic;

public static class DispenserData
{
    public enum DispenserType
    {
        None = 0,
        Normal,
        LaserBeam,
        Missile,
        Railgun,
        RepairKit,
        Fuel,
    }

    public static readonly Dictionary<DispenserType, Color> ColorByItemType = new Dictionary<DispenserType, Color>(5)
    {
        { DispenserType.None, Color.white},
        { DispenserType.Normal, Color.green},
        { DispenserType.LaserBeam, Color.red},
        { DispenserType.Missile, Color.green},
        { DispenserType.Railgun, Color.blue},
        { DispenserType.RepairKit, Color.green},
        { DispenserType.Fuel, Color.yellow},
    };
}

public class Dispenser : MonoBehaviour
{
    [SerializeField] private DispenserData.DispenserType _type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (!player) 
            {
                Debug.LogError($"Dispenser {gameObject.name} failed to find player");
                return;
            }

            switch (_type)
            {
                case DispenserData.DispenserType.Fuel:
                    if (DispenserData.ColorByItemType.TryGetValue(_type, out Color itemColor))
                    {
                        player.PickUpFuel(itemColor);
                    }
                    break;
                case DispenserData.DispenserType.LaserBeam:
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            switch (_type)
            {
                case DispenserData.DispenserType.Fuel:
                    break;
            }
        }
    }
}
