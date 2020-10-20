using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "Dispenser/Dispenser")]
public class DispenserItem : ScriptableObject
{
    [SerializeField] private DispenserData.Type _type = DispenserData.Type.None;
    //[SerializeField] private Color color = Color.white; 
    [SerializeField] private GameObject _itemPrefab = null;
    [SerializeField] private Sprite _dispenserSprite = null;
    [SerializeField] private Sprite _itemSprite = null;

    public DispenserData.Type Type => _type;
    public GameObject ItemPerfab => _itemPrefab;
    public Sprite DispenserSprite => _dispenserSprite;
    public Sprite ItemSprite => _itemSprite;

    //public DispenserData.Type DispenserType { get { return type; } set { type = value; } }
    //public Color DispenserColor { get { return color; } set { color = value; } }
    //public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }
    //public Sprite sprite { get { return sp; }set { sp = value; } }
    //public Sprite Objectsprite { get { return Objectsp; } set { Objectsp = value; } }
}