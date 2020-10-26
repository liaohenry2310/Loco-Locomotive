using Interfaces;
using System.Collections;
using System.Collections.Generic;
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

        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection);

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion q = Quaternion.AngleAxis(angle - 80.0f, Vector3.forward);
        ////transform.rotation = q;
        //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

        if (currentPos.y <= screenBottom)
        {
            gameObject.SetActive(false);

        }
        //float dis = Vector3.Distance(currentPos, targetPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ParticleSystem>())
        {
            return; // when release the EMP just ignore the bullets
        }

        if (collision.CompareTag("Bullet")) // When bullet comming from Turret, just set false the enemy bullet
        {
            gameObject.SetActive(false);
        }


        IDamageable<float> damageable = collision.GetComponentInParent<IDamageable<float>>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }


        if (collision.gameObject.GetComponentInParent<Train>())
        {
            collision.gameObject.GetComponentInParent<Train>().TakeDamage(damage);
            //Debug.Log("basic doing damage to train");
            gameObject.SetActive(false);
        }

        if (collision.gameObject.GetComponent<TurretHealth>())
        {
            collision.gameObject.GetComponent<TurretHealth>().TakeDamage(damage);
            Debug.Log("basic doing damage to Turret");
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Debug.Log("basic doing damage to Player");
            gameObject.SetActive(false);
        }

        var shieldGen = collision.GetComponent<ShieldTurret>(); // when bullet hit the ShieldGenerator GameObject
        if (shieldGen)
        {
            shieldGen.IDamageble.TakeDamage(damage);
            gameObject.SetActive(false);
        }

        var empGen = collision.GetComponent<EMPTurret>(); // when bullet hit the EMPGenerator GameObject
        if (empGen)
        {
            empGen.IDamageable.TakeDamage(damage);
            gameObject.SetActive(false);
        }
        //  if (!collision.gameObject.GetComponent<TurretHealth>() || !collision.gameObject.GetComponentInParent<TrainHealth>())
        //  {
        //      collision.isTrigger = false;
        //  }
        // else 
        // {
        //     //  Debug.Log("BasicEnemy Projectile set to false");
        //     //  gameObject.SetActive(false);
        // }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("BasicEnemy Projectile set to false");
            gameObject.SetActive(false);
        }
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        //    Debug.Log("basic doing damage to Player");
        //    gameObject.SetActive(false);
        //}

    }


}
