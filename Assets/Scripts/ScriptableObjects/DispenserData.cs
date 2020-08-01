using UnityEngine;

public static class DispenserData
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
}

public class DispenserItemData
{
    public DispenserData.Type itemType;
    public Color itemColor = Color.white;
    public GameObject itemPrefab = null;
}