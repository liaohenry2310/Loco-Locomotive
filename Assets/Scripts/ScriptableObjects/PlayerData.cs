using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataObject", menuName = "Player/Player")]
public class PlayerData : ScriptableObject
{
    public LayerMask InteractableMask = 0;
    [Range(0.0f, 1.0f)]
    public float Radius = 0.5f;
    [Range(1.0f, 10.0f)]
    public float Speed = 3.0f;
    public float Gravity = 5.0f;
    public float MaxHealth = 100.0f;
}
