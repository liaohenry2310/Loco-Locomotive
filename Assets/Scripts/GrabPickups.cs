using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Security.Cryptography;
using System.Collections.Specialized;
using UnityEngine;

public class GrabPickups : MonoBehaviour
{
    public bool canPickup = true;
    public bool isHolding = false;

    public float distance;
    public float throwForce = 100.0f;

    public GameObject bullet;

    private InputReciever mInputReciever;
    private Rigidbody2D mRigidBody;

    void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mInputReciever = GetComponent<InputReciever>();

        bullet.SetActive(false);
    }

    void Update()
    {
        if (canPickup = true) 
        {
            bullet.SetActive(true);

            if (mInputReciever.GetPrimaryInput())
            {
                isHolding = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag=="Player")
        {
            mInputReciever.GetHorizontalInput();
        }
    }
}
