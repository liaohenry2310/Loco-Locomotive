using UnityEngine;

public class Bullet : ProjectileAmmo
{
    private Vector3 screenBouds;

    void Start()
    {
        screenBouds = GameManager.GetScreenBounds;
        AmmoType = DispenserData.Type.Normal;
    }

    private void Update()
    {
        if (AmmoTypeData.MoveSpeed != 0f)
        {
            transform.position += transform.up * (AmmoTypeData.MoveSpeed * Time.deltaTime);

            // Destroy prefabs when touch the camera bounds
            if ((transform.position.x >= screenBouds.x) ||
                (transform.position.x <= -screenBouds.x) ||
                (transform.position.y >= screenBouds.y) ||
                (transform.position.y <= -screenBouds.y))
            {
                gameObject.SetActive(false);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable<float> damageable = collision.GetComponentInParent<EnemyHealth>();
        if (damageable != null)
        {
            damageable.TakeDamage(AmmoTypeData.Damage, AmmoType);
            gameObject.SetActive(false);
        }
        // Old code
        //if (collision.CompareTag("Enemy"))
        //{
        //    BasicEnemy enemy = collision.gameObject.GetComponentInParent<BasicEnemy>();
        //    enemy.TakeDamage(_bulletData.Damage);
        //    gameObject.SetActive(false);
        //}
    }

}
