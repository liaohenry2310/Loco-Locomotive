using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InputReciever : MonoBehaviour
{
    private Vector2 mInputDirectional;
    private bool mInputPrimary;
    private bool mInputPrimaryHold;
    private bool mInputSecondary;
    private bool mInputSecondaryHold;

    public void SetInput(Vector2 directional, bool primary, bool primaryHold, bool secondary, bool secondaryHold)
    {
        mInputDirectional = directional;
        mInputPrimary = primary;
        mInputPrimaryHold = primaryHold;
        mInputSecondary = secondary;
        mInputSecondaryHold = secondaryHold;
    }

    public void SetZeroInput()
    {
        mInputDirectional = Vector2.zero;
        mInputPrimary = false;
        mInputPrimaryHold = false;
        mInputSecondary = false;
        mInputSecondaryHold = false;
    }

    public Vector2 GetRawDirectionalInput()
    {
        return mInputDirectional;
    }

    public float GetRawHorizontalInput()
    {
        return mInputDirectional.x;
    }

    public float GetRawVerticalInput()
    {
        return mInputDirectional.y;
    }

    public bool GetPrimaryInput()
    {
        return mInputPrimary;
    }

    public bool GetPrimaryHoldInput()
    {
        return mInputPrimaryHold;
    }

    public bool GetSecondaryInput()
    {
        return mInputSecondary;
    }

    public bool GetSecondaryHoldInput()
    {
        return mInputSecondaryHold;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().InputReceiver = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().InputReceiver = null;
        }
    }
}
