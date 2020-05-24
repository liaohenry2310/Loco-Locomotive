using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReciever : MonoBehaviour
{
    private PlayerInput mPlayerInput;
    private bool mInUse = false;
    private bool mIsPlayer = false;

    private bool mPrimaryHeld = false;
    private bool mSecondaryHeld = false;

    public bool SetPlayerInput(ref PlayerInput playerInput)
    {
        if (!mInUse)
        {
            mPlayerInput = playerInput;
            mInUse = true;
            mPlayerInput.actions["PrimaryHold"].started += ctx => mPrimaryHeld = true;
            mPlayerInput.actions["PrimaryHold"].canceled += ctx => mPrimaryHeld = false;
            mPlayerInput.actions["SecondaryHold"].started += ctx => mSecondaryHeld = true;
            mPlayerInput.actions["SecondaryHold"].canceled += ctx => mSecondaryHeld = false;
            return true;
        }
        return false;
    }

    public bool DisablePlayerInput()
    {
        if (mInUse)
        {
            mPlayerInput = null;
            mInUse = false;
            mPrimaryHeld = false;
            mSecondaryHeld = false;
            return true;
        }
        return false;
    }

    public Vector2 GetDirectionalInput()
    {
        if (mPlayerInput)
        {
            return mPlayerInput.actions["Directional"].ReadValue<Vector2>();
        }
        else
        {
            return Vector2.zero;
        }
    }

    public float GetHorizontalInput()
    {
        if (mPlayerInput)
        {
            return mPlayerInput.actions["Directional"].ReadValue<Vector2>().x;
        }
        else
        {
            return 0.0f;
        }
    }

    public float GetVerticalInput()
    {
        if (mPlayerInput)
        {
            return mPlayerInput.actions["Directional"].ReadValue<Vector2>().y;
        }
        else
        {
            return 0.0f;
        }
    }

    public bool GetPrimaryInput()
    {
        if (mPlayerInput)
        {
            return mPlayerInput.actions["Primary"].triggered;
        }
        else
        {
            return false;
        }
    }

    public bool GetPrimaryHoldInput()
    {
        return mPrimaryHeld;
    }

    public bool GetSecondaryInput()
    {
        if (mPlayerInput)
        {
            return mPlayerInput.actions["Secondary"].triggered;
        }
        else
        {
            return false;
        }
    }

    public bool GetSecondaryHoldInput()
    {
        return mSecondaryHeld;
    }

    public bool IsUsingGamepad()
    {
        return mPlayerInput.defaultControlScheme.Equals("Gamepad");
    }


    private void Start()
    {
        if (GetComponent<Player>())
        {
            mIsPlayer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!mIsPlayer && collision.CompareTag("Player"))
        {
            InputReciever inputReciever = this;
            collision.GetComponent<Player>().PlayerController.SetControllable(ref inputReciever);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!mIsPlayer && collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().PlayerController.SetNoControllable();
        }
    }
}
