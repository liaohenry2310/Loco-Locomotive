using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Security.Cryptography;
using System.Collections.Specialized;
using UnityEngine;

public class GrabPickups : MonoBehaviour
{
    private InputReciever mInputReciever;
    RaycastHit2D hit;
    public bool grabbed;
    public float distance = 0.5f;
    public Transform holdpoint;
    public float throwforce = 10;
    public float destorytime=10.0f;

    void Start()
    {
        mInputReciever = GetComponent<InputReciever>();
    }

    void Update()
    {

        if (mInputReciever.GetPrimaryHoldInput())
        {

            if (!grabbed)
            {
                Physics2D.queriesStartInColliders = false;

                hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

                if (hit.collider != null && hit.collider.tag == "grabbable")
                {
                    grabbed = true;

                }

            }
            else if (!Physics2D.OverlapPoint(holdpoint.position))
            {
                grabbed = false;

                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {

                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwforce;
                    Destroy(this.gameObject, destorytime);
                }

            }
        }
		if (grabbed)
                        hit.collider.gameObject.transform.position = holdpoint.position;
}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }



    //public bool canPickup = true;
    //public bool isHolding = false;

    //public float distance;
    //public float throwForce = 100.0f;

    //public GameObject bullet;

    //private InputReciever mInputReciever;
    //private Rigidbody2D mRigidBody;

    //void Start()
    //{
    //    mRigidBody = GetComponent<Rigidbody2D>();
    //    mInputReciever = GetComponent<InputReciever>();

    //    bullet.SetActive(false);
    //}

    //void Update()
    //{
    //    if (canPickup == true) 
    //    {
    //        bullet.SetActive(true);

    //        if (mInputReciever.GetPrimaryInput())
    //        {
    //            isHolding = true;
    //        }
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.tag=="Player")
    //    {
    //        mInputReciever.GetHorizontalInput();
    //    }
    //}
}
