using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ---- LEGACY CODE -----
/// I will delete this script in the FUTURE.
/// Cyro.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Player mPlayerAvatar;
    private InputReciever mPlayerAvatarReciver;
    private InputReciever mPlayerControllableReciever;

    private PlayerInput mPlayerInput;
    private bool mIsUsingControllable;

    public void SetPlayer(Player player)
    {
        mPlayerAvatar = player;
        mPlayerAvatarReciver = mPlayerAvatar.GetComponent<InputReciever>();
        mPlayerAvatarReciver.SetPlayerInput(GetComponent<PlayerInput>());
        mPlayerAvatar.PlayerController = this;
    }

    public void SetControllable(InputReciever controllable)
    {
        mPlayerControllableReciever = controllable;
    }

    public void SetNoControllable()
    {
        mPlayerControllableReciever = null;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mPlayerInput = GetComponent<PlayerInput>();
        mIsUsingControllable = false;
    }

    private void Update()
    {
        if (mPlayerControllableReciever &&
            !mIsUsingControllable &&
            mPlayerInput.actions["Primary"].triggered &&
            mPlayerControllableReciever.SetPlayerInput(mPlayerInput))
        {
            mIsUsingControllable = true;
            mPlayerAvatarReciver.DisablePlayerInput();
        }
        else if (mPlayerControllableReciever &&
            mIsUsingControllable &&
            mPlayerInput.actions["Primary"].triggered &&
            mPlayerAvatarReciver.SetPlayerInput(mPlayerInput))
        {
            mIsUsingControllable = false;
            mPlayerControllableReciever.DisablePlayerInput();
        }
    }
}
