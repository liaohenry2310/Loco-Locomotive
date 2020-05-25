using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed = 10f;
    public float Damage = 5f;
    private Vector3 screenBouds;

    void Start()
    {
        screenBouds = FindObjectOfType<GameManager>().ScreenBounds;
    }

    void Update()
    {
        if (Speed != 0f)
        {
            transform.position += transform.up * (Speed * Time.deltaTime);

            // Destroy prefabs when touch the camera bounds
            if ((transform.position.x >= screenBouds.x) ||
                (transform.position.x <= -screenBouds.x) ||
                (transform.position.y >= screenBouds.y) ||
                (transform.position.y <= -screenBouds.y))
            {
                Destroy(gameObject);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<BasicEnemy>().name == "BasicEnemy(Clone)")
        {
            BasicEnemy enemy = collision.gameObject.GetComponentInParent<BasicEnemy>();
            enemy.TakeDamage(Damage);
        }
    }

}
