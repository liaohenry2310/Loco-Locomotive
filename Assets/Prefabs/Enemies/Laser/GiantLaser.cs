using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantLaser : MonoBehaviour
{

    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public GameObject VFX;
    public bool isfire = false;
    public bool isfireUpdate = false;
    public Transform tarpos;

    private List<ParticleSystem> particles = new List<ParticleSystem>();
    // Start is called before the first frame update
    void Start()
    {
        FillLists();
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (isfire)
        {
            EnableLaser();
        }
        if (isfire&& isfireUpdate)
        {
            UpdateLaser();
        }
        if (!isfire)
        {
            DisableLaser();
        }
    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;
        for (int i = 0; i < particles.Count; ++i)
        {
            particles[i].Play();
        }

    }
   void UpdateLaser()
    {
        var pos = (Vector2)tarpos.position;
        lineRenderer.SetPosition(0, firePoint.position);
        VFX.transform.position = (Vector2)firePoint.position;
        lineRenderer.SetPosition(1, (Vector2)tarpos.position);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, pos.normalized, pos.magnitude);
        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
    }
   void DisableLaser() {
        lineRenderer.enabled = false;
        for (int i = 0; i < particles.Count; ++i)
        {
            particles[i].Play();
        }
    }

    void FillLists()
    {
        for (int i = 0; i < VFX.transform.childCount; ++i)
        {
            var ps = VFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps!=null)
            {
                particles.Add(ps);
            }
        }
    }
}
