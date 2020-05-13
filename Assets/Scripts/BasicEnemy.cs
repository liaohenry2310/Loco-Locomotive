using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    // Basic Enemy Stuffs....
    public float health;
    public float damage;
    public float gravity = 1.0f;
    public GameObject target; 
 
    private Vector3 targetPos;
    private Vector3 currentPos;
    private Vector3 direction;
    
    public GameObject groundArea;


    private float mTakeDamageDelay=1.5f;

    // Change Enemy State .
    private State mCurrentState = State.Nothing;
    enum State
    {
        InSky,
        OnTrain,
        OnGround,
        Nothing
    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {



        targetPos =(target.transform.localPosition);
        currentPos =transform.position;
        direction = targetPos - currentPos;
        direction.Normalize();

        if (mCurrentState == State.InSky)
        {
            transform.Translate(Vector2.down * gravity * Time.deltaTime);
        }
        if (mCurrentState == State.OnTrain)
        {
    
            transform.Translate(direction * gravity * Time.deltaTime,Space.World);

        }
        if (mCurrentState == State.OnGround)
        {
            // Delay damege taking, make sure is not gone right away .
            //  delataTime is time of the frame,if fps is 60, deltaTime is frame/60sec.
            //  time is the total time of the game is running for.
            if (mTakeDamageDelay<Time.time )
            {
                mTakeDamageDelay = Time.time+1.5f;
                TakeDamage(5.0f);
            }
        }
    }

    void TakeDamage(float takingDamage)
    {
        health -= takingDamage;
        if (health <= 0.0f)
        {
            Destroy(gameObject);
            Debug.Log("See you next time!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.transform.parent.gameObject.CompareTag("Train"))
        {
            mCurrentState = State.OnTrain;
    
        }
        if (collision.gameObject == groundArea)
        {
            mCurrentState = State.OnGround;
        }
    }
}
