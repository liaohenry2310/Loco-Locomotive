using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemy : MonoBehaviour
{
    //damage
    public float damage = 10.0f;
    public float speed = 5.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<TrainHealth>())
        {
            collision.gameObject.GetComponentInParent<TrainHealth>().TakeDamage(damage);
            Debug.Log("Train taking damage");
            Destroy(gameObject); 
        }

        if (collision.gameObject.GetComponent<TurretHealth>())
        {
            collision.gameObject.GetComponent<TurretHealth>().TakeDamage(damage);
            Debug.Log(" Turret taking damage");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Debug.Log("Player taking damage");
            Destroy(gameObject);
        }
    }

    public Vector3 UpdatePos(Vector3 direction)
    {
        Vector3 velocity = Vector3.zero;
        velocity=SeekBehaviour.SeekMove(transform, (Vector2)direction, speed);
        return velocity;
    }
}
