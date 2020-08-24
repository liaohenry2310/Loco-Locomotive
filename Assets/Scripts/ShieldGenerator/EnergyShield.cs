using System;
using UnityEngine;

public class EnergyShield : MonoBehaviour
{
    [NonSerialized] private Color _color;

    private void Start()
    {
        _color = Color.blue;
        _color.a = 0.2f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(gameObject.transform.position, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}