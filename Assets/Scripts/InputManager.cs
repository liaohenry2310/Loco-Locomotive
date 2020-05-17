using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum EInputCommand
    {
        Train, Player
    }

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Transform train;

    public EInputCommand InputCommand { get; set; }

    private void Awake()
    {
        InputCommand = EInputCommand.Player;
    }


}
