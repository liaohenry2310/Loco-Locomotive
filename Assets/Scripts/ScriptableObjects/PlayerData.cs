using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataObject", menuName = "Player/Player")]
public class PlayerData : ScriptableObject
{
    public LayerMask InteractableMask = 0;
    [Range(0.0f, 1.0f)]
    [Tooltip("Radius to player interact with others objects")]
    public float Radius = 0.5f;
    [Range(1.0f, 10.0f)]
    [Tooltip("Player movement speed")]
    public float Speed = 3.0f;
    public float Gravity = 5.0f;
    public float MaxHealth = 100.0f;
    [Tooltip("Rate regeneration per seconds")]
    public float HealthRateRegen = 25.0f;
    [Tooltip("Allow to set how much time to not take damage to start to regenerate")]
    public float DelayTimeRegen = 3.0f; 

    public AudioClip[] AudiosClips;

}
