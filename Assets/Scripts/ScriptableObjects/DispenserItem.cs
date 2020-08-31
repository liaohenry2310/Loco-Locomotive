using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "DataObject/Dispenser")]
public class DispenserItem : ScriptableObject
{
    [SerializeField] private DispenserData.Type type = DispenserData.Type.None;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private Sprite sp = null;
    [SerializeField] private Sprite Objectsp = null;


    public DispenserData.Type DispenserType { get { return type; } set { type = value; } }
    public Color DispenserColor { get { return color; } set { color = value; } }
    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }
    public Sprite sprite { get { return sp; }set { sp = value; } }
    public Sprite Objectsprite { get { return Objectsp; } set { Objectsp = value; } }

}