using UnityEngine;

[CreateAssetMenu(fileName = "DispenserDataObject", menuName = "Dispenser/Dispenser")]
public class DispenserItem : ScriptableObject
{
    [SerializeField] private DispenserData.Type _type = DispenserData.Type.None;
    [SerializeField] private GameObject _itemPrefab = null;
    [SerializeField] private Sprite _dispenserSprite = null;
    [SerializeField] private Sprite _itemSprite = null;

    public DispenserData.Type Type => _type;
    public GameObject ItemPerfab => _itemPrefab;
    public Sprite DispenserSprite => _dispenserSprite;
    public Sprite ItemSprite => _itemSprite;
}