using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy:MonoBehaviour
{
    public List<GameObject> targetList;
    public GameObject landingCollider;
    public GameObject trainArea;
   // public Vector3 currentPos;
    //public Vector3 screenBounds;
    //public float leftRange;
    //public float rightRange;
    //public float screenBottom;
    //private void Start()
    //{
    //    screenBounds = GameManager.GetScreenBounds;
    //    rightRange = screenBounds.x + 3.0f;
    //    leftRange = -screenBounds.x - 3.0f;
    //    screenBottom = -screenBounds.y;
    //}
    //
    //public bool OutsideScreen()
    //{
    //    return false;//currentPos.y>= screenBottom;
    //}

}
