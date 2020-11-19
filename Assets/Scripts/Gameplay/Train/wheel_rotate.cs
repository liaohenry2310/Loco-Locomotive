using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel_rotate : MonoBehaviour
{
    public float _RotationSpeed;
    void Update()
    {
        this.transform.Rotate(Vector3.back, 45*_RotationSpeed * Time.deltaTime, Space.Self);
    }
}
