using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Player mPlayerAvatar;
    private InputReciever mPlayerAvatarReciver;
    private InputReciever mPlayerControllableReciever;

    private PlayerInput mPlayerInput;
    private bool mIsUsingControllable;

    public void SetPlayer(ref Player player)
    {
        mPlayerAvatar = player;
        mPlayerAvatarReciver = mPlayerAvatar.GetComponent<InputReciever>();
        mPlayerAvatarReciver.SetPlayerInput(ref mPlayerInput);
        mPlayerAvatar.PlayerController = this;
    }

    public void SetControllable(ref InputReciever controllable)
    {
        mPlayerControllableReciever = controllable;
    }

    public void SetNoControllable()
    {
        mPlayerControllableReciever = null;
    }

    private void Awake()
    {
        mPlayerInput = GetComponent<PlayerInput>();
        mIsUsingControllable = false;
    }

    private void Update()
    {
        if (mPlayerControllableReciever &&
            !mIsUsingControllable && 
            mPlayerInput.actions["Primary"].triggered && 
            mPlayerControllableReciever.SetPlayerInput(ref mPlayerInput))
        {
            mIsUsingControllable = true;
            mPlayerAvatarReciver.DisablePlayerInput();
        }
        else if (mPlayerControllableReciever && 
            mIsUsingControllable && 
            mPlayerInput.actions["Primary"].triggered && 
            mPlayerAvatarReciver.SetPlayerInput(ref mPlayerInput))
        {
            mIsUsingControllable = false;
            mPlayerControllableReciever.DisablePlayerInput();
        }
    }
}
