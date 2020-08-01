using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "DataObject/Dispenser")]
public class DispenserItem : ScriptableObject
{
    [SerializeField] private DispenserData.Type _type = default;
    [SerializeField] private Color _color = default;

    public DispenserData.Type DispenserType { get { return _type; } } // set { type = value; } 
    public Color DispenserColor { get { return _color; } }
}