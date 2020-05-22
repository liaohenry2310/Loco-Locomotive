using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed = 10f;
    private Vector3 screenBouds;

    void Start()
    {
        screenBouds = FindObjectOfType<GameManager>().ScreenBounds;
    }

    void Update()
    {
        if (Speed != 0f)
        {
            transform.position += transform.right * (Speed * Time.deltaTime);

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
}
