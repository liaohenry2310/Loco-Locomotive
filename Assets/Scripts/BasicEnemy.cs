using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    // Basic Enemy Stuffs....

    // Drop down speed
    public float fallSpeed = 1.0f;
    public float gravity = 1.0f;
    // .....
    public float windSpeed=1.5f;

    // Health
    public float health;

    // Damage
    public float damage;

    // Target
    public List<GameObject> targetList;
    private GameObject currentTarget;
    private Vector3 targetPos;
    private Vector3 direction;

    //Train Landing area
    public GameObject topWagonCollider;

    // Pos. 
    private Vector3 currentPos;

    // m2D Collider 
    private Vector2 mColliderSize;

    // Check Gound
    public GameObject groundArea;

    // Bounding Check
    private Camera MainCam;
    private Vector2 screenBounds;
    private float leftRange;
    private float rightRange;

    // Attack Delay 
    private float mTakeDamageDelay=1.5f;

    // Change Enemy State .
    private State mCurrentState = State.InSky;
    enum State
    {
        InSky,
        OnTrain,
        OnGround,
        OnAttack,
        Nothing
    }
    private void Start()
    {
        // Bounding check
        MainCam = FindObjectOfType<Camera>();
        screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
        rightRange = screenBounds.x + 1.0f;
        leftRange = -screenBounds.x + 1.0f;


        mColliderSize = GetComponentInChildren<BoxCollider2D>().size;
    }
    // Update is called once per frame
    void Update()
    {

       currentPos =transform.position;
       direction = targetPos - currentPos;
       direction.Normalize();
       float dis = Vector3.Distance(currentPos, targetPos);

        if (mCurrentState == State.InSky)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            transform.position = new Vector2(transform.position.x- windSpeed * Time.deltaTime, transform.position.y - fallSpeed * Time.deltaTime);
        }
        
        if (mCurrentState == State.OnTrain)
        {
            GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            transform.Translate(direction * gravity * Time.deltaTime,Space.World);
            GetTargetPosition();

            if (dis<= mColliderSize.x)
            {
                transform.position = currentPos;
                mCurrentState = State.OnAttack;
            }

        }
         if(mCurrentState == State.OnAttack)
        {
            if (mTakeDamageDelay < Time.time)
            {
                mTakeDamageDelay = Time.time + 0.5f;

                //currentTarget.GetComponent<TurretHealth>().TakeDamage(10.0f);
            }

        }

        // Check if outside of screen 
        OffScreen();

    }



    public void TakeDamage(float takingDamage)
    {
        health -= takingDamage;
        if (health <= 0.0f)
        {
            Destroy(gameObject);
            Debug.Log("See you next time!");
        }
    }

    // Check if landed on train
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == topWagonCollider)
        {
            transform.parent = topWagonCollider.gameObject.transform;
            mCurrentState = State.OnTrain;
        }
    }

    // Check if landed on ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == groundArea)
        {
            mCurrentState = State.OnGround;
            Destroy(gameObject);
        }
    }

    // Check if outside of screen 
    private void OffScreen()
    {

        if (transform.position.x < leftRange )
           // || transform.position.x >rightRange )
        {
            mCurrentState = State.Nothing;
            Destroy(gameObject);
        }
    }

    private void GetTargetPosition()
    {
        float distance = Vector2.Distance(transform.position, targetList[0].transform.position);
        targetPos = (targetList[0].transform.position);
        currentTarget = targetList[0];

        foreach (var target in targetList)
        {

            if (Vector2.Distance(transform.position,target.transform.position)
                < 
                distance)
            {
                targetPos = (target.transform.position);
                distance = Vector2.Distance(transform.position, target.transform.position);
                currentTarget = target;
            }

        }
    }
}
