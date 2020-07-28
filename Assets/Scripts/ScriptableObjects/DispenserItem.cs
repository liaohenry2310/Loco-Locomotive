using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "Dispenser")]
public class DispenserItem : ScriptableObject
{

    [SerializeField] private DispenserData.Type type = default;
    [SerializeField] private Color color = default;
    [SerializeField] private GameObject itemPrefab =  null;

    public DispenserData.Type DispenserType { get { return type; } set{ type = value; } } 
    public Color DispenserColor { get { return color; } set { color = value; } }

    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }
}