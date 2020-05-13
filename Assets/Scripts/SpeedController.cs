using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    public float maxSpeed;
    private float mMoveSpeed;
    private float mMinSpeed=0.1f;
    private float mInitSpeed=0.0f;

    public GameObject gameObj;
    public bool controller;

    public Camera MainCam;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        // Check View Bounding
        screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
        objectWidth = gameObj.transform.localScale.x/2;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 position = gameObj.gameObject.transform.position;
        var normalSpeed = mMoveSpeed/2;
        if (controller == true )//&& position.x>= horizontalMin)
        {
            mMoveSpeed = maxSpeed;
            if (position.x >= (screenBounds.x-objectWidth))
            {
                controller = false;
                Debug.Log("Right bounding check");
            }
            gameObj.transform.Translate(Vector2.right * mMoveSpeed * Time.deltaTime);
        }

        if (controller == false)
        {

            gameObj.transform.Translate(-Vector2.right * normalSpeed * Time.deltaTime);
            if (position.x <= (-screenBounds.x + objectWidth))
            {
                controller = true;
                Debug.Log("Left bounding check");

            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
               controller = true;
            Debug.Log("Controller On");
        }
        //if (collision.CompareTag("Player") && ONorOFF == true)
        //{
        //    ONorOFF = false;
        //    Debug.Log("Controller Off");
        //}
    }
  

   
}
