using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "Dispenser")]
public class DispenserItem : ScriptableObject
{
    [SerializeField] private DispenserData.Type type = default;
    [SerializeField] private Color color = default;

    public DispenserData.Type DispenserType { get { return type; } } // set { type = value; } 
    public Color DispenserColor { get { return color; } }
}