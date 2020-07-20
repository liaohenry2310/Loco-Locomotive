using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "Dispenser")]

public class DispenserItem : ScriptableObject
{
    [SerializeField] private DispenserData.Type type;
    [SerializeField] private Color color;

    public DispenserData.Type DispenserType { get { return type; } set { type = value; } }
    public Color DispenserColor { get { return color; } }
}