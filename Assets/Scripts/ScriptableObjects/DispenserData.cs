using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "Dispenser")]
public class DispenserData : ScriptableObject
{
    public enum Type
    {
        None = 0,
        Normal,
        LaserBeam,
        Missile,
        Railgun,
        RepairKit,
        Fuel,
    }

    public readonly Dictionary<Type, Color> ColorByItemType = new Dictionary<Type, Color>(7)
    {
        { Type.None, Color.white},
        { Type.Normal, Color.green},
        { Type.LaserBeam, Color.red},
        { Type.Missile, Color.green},
        { Type.Railgun, Color.blue},
        { Type.RepairKit, Color.cyan},
        { Type.Fuel, Color.yellow},
    };

}
