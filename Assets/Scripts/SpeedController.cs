using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    public float moveSpeed;
    private float InitSpeed=0.0f;

    public GameObject gameObj;
    public bool controller;

    public Camera MainCam;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 position = gameObj.gameObject.transform.position;

        if (controller == true )//&& position.x>= horizontalMin)
        {
            if (position.x >= (screenBounds.x))
            {
                controller = false;
            }
            gameObj.transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        if (controller == false )
        {
            gameObj.transform.Translate(-Vector2.right * moveSpeed/2 * Time.deltaTime);
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
