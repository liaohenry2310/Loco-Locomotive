using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{
    public float speed;
    public Vector2 targetPos;
    Vector2 direction;
    Vector2 currentPos;
    public float damage;

    float screenBottom;

    void Start()
    {
        //Camera MainCam = Camera.main;
        //screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
        screenBottom = -GameManager.GetScreenBounds.y;

    }

    void Update()
    {

        currentPos = gameObject.transform.position;
        direction = targetPos - currentPos;
        direction.Normalize();
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (currentPos.y <= screenBottom)
        {
            gameObject.SetActive(false);

        }
        //float dis = Vector3.Distance(currentPos, targetPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Missile")) return; // this line the bullet will not disappear when collide with the missile


        if (collision.gameObject.CompareTag("Train")) //|| collision.gameObject.CompareTag("Train"))
        {
            collision.gameObject.GetComponentInParent<TrainHealth>().TakeDamage(damage);
            //Debug.Log("basic doing damage to train");
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Turret"))
        {

            collision.gameObject.GetComponent<TurretHealth>().TakeDamage(damage);
            Debug.Log("basic doing damage to Turret");
            gameObject.SetActive(false);
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("BasicEnemy Projectile set to false");
            gameObject.SetActive(false);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("BasicEnemy Projectile set to false");
            gameObject.SetActive(false);
        }

    }


}
