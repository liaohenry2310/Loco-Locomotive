using System.Collections;
using UnityEngine;

public class wheel_rotate : MonoBehaviour
{
    [SerializeField] private float _RotationSpeed;

    private void Start()
    {
        StartCoroutine(KeepingRotate());
    }

    private IEnumerator KeepingRotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.back, 45f * _RotationSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }


    //void Update()
    //{
    //    this.transform.Rotate(Vector3.back, 45 * _RotationSpeed * Time.deltaTime, Space.Self);
    //}
}
