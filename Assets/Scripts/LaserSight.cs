using UnityEngine;

public class LaserSight : MonoBehaviour
{
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(transform.position, transform.right, out hit))
        //{
        //    if (hit.collider)
        //    {
        //        lineRenderer.SetPosition(1, new Vector3(0f, 0f, hit.distance));
        //    }
        //    else
        //    {
        //        lineRenderer.SetPosition(1, new Vector3(0f, 0f, 5000f));
        //    }

        //}
    }
}
