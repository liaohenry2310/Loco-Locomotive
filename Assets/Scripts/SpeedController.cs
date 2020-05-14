using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    //public float maxSpeed;
    //private float mMoveSpeed;
    //private float mMinSpeed = 0.1f;
    //private float mInitSpeed = 0.0f;

    //public GameObject gameObj;
    //public bool controller;

    //public Camera MainCam;
    //private Vector2 screenBounds;
    //private float objectWidth;
    //private float objectHeight;

    [SerializeField]
    private Transform trainTransform;

    [SerializeField]
    private float TrainSpeed = 10f;

    private bool IsControllSetToLeft = false;

    private void Awake()
    {
        Debug.Log($"comprimento: {Screen.width}");
        Debug.Log($"altura: {Screen.height}");

        // Check View Bounding
        //screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
        //objectWidth = gameObj.transform.localScale.x / 2;
    }


    private void Update()
    {

        //Vector2 position = gameObj.gameObject.transform.position;
        //var normalSpeed = mMoveSpeed / 2;
        //if (controller == true)//&& position.x>= horizontalMin)
        //{
        //    mMoveSpeed = maxSpeed;
        //    if (position.x >= (screenBounds.x - objectWidth))
        //    {
        //        controller = false;
        //        Debug.Log("Right bounding check");
        //    }
        //    gameObj.transform.Translate(Vector2.right * mMoveSpeed * Time.deltaTime);
        //}

        //if (controller == false)
        //{

        //    gameObj.transform.Translate(-Vector2.right * normalSpeed * Time.deltaTime);
        //    if (position.x <= (-screenBounds.x + objectWidth))
        //    {
        //        controller = true;
        //        Debug.Log("Left bounding check");

        //    }
        //}
        //TODO: preciso de mais testes
        if (IsControllSetToLeft)
        {
            trainTransform.position -= new Vector3(TrainSpeed * Time.deltaTime, trainTransform.position.y);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            IsControllSetToLeft = !IsControllSetToLeft;
            Debug.Log($"Controller On: {IsControllSetToLeft}");
        }
    }
}
