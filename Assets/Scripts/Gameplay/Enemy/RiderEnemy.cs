using GamePlay;
using UnityEngine;

public class RiderEnemy : Enemy
{

    // Basic Enemy Stuffs....

    // Drop down speed
    public float fallSpeed = 1.0f;
    public float gravity = 1.0f;
    // .....
    public float windSpeed = 1.5f;

    // Health
    //public float health;

    //#region Shield Modifier
    //[SerializeField]
    //ShieldModifier _shieldModifier;
    //public bool IsHasShield => _shieldModifier;
    //
    //#endregion


    // Damage
    public float damage;

    // Target
  //  public List<GameObject> targetList;
    private GameObject currentTarget;
    private Vector3 targetPos;
    private Vector3 direction;

    //Train Landing area
   // public GameObject topWagonCollider;

    // Pos. 
    private Vector3 currentPos;
    public Train trainHealth;

    private Vector2 mColliderSize;

    //// Check Gound
    //public GameObject groundArea;

    // Bounding Check
    private Camera MainCam;
    private Vector2 screenBounds;
    private float leftRange;
    private float rightRange;
    float screenBottom;

    // Attack Delay 
    private float mTakeDamageDelay = 1.5f;

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
        rightRange = screenBounds.x + 3.0f;
        leftRange = -screenBounds.x - 3.0f;
        screenBottom = -screenBounds.y;
        mColliderSize = GetComponentInChildren<BoxCollider2D>().size;
    }
    // Update is called once per frame
    void Update()
    {
        // check if is hitting the bottom of the screen.
        if (OutsideScreen())
        {
            gameObject.SetActive(false);

        }

        currentPos = transform.position;


        if (mCurrentState == State.InSky)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            transform.position = new Vector2(transform.position.x - windSpeed * Time.deltaTime, transform.position.y - fallSpeed * Time.deltaTime);
        }

        if (mCurrentState == State.OnTrain)
        {
            GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            GetTargetPosition();
            direction = targetPos - currentPos;
            direction.Normalize();
            transform.Translate(direction * gravity * Time.deltaTime, Space.World);

            float dis = Vector3.Distance(currentPos, targetPos);

            if (dis <= mColliderSize.x)
            {
                transform.position = currentPos;

                mCurrentState = State.OnAttack;
            }

        }
        if (mCurrentState == State.OnAttack)
        {
            if (mTakeDamageDelay < Time.time)
            {
                mTakeDamageDelay = Time.time + 1.5f;
                if (currentTarget != null && currentTarget.gameObject.tag == "Turret")
                {
                    currentTarget.GetComponent<TurretHealth>().TakeDamage(10.0f);
                    Debug.Log("Turret taking damage");

                }
                if (currentTarget != null && currentTarget.gameObject.tag == "FrontWagon")
                {
                    currentTarget.GetComponentInParent<Train>().TakeDamage(10.0f);
                    Debug.Log("FrontWagon taking damage");
                }

                // " ? " < -- Ternary operator  " if not currentTarget not equal the first script , then go to the second one . "

                if (currentTarget?.GetComponent<TurretHealth>()?.IsAlive == false)// || currentTarget?.GetComponentInParent<Train>()?.IsAlive() == false )
                {
                    currentTarget = null;
                }
                
                if (currentTarget == null)
                {
                    mCurrentState = State.OnTrain;
                }
            }

        }

        // Check if outside of screen 
        OffScreen();

    }



    //public void TakeDamage(float takingDamage)
    //{
    //    health -= takingDamage;
    //    if (health <= 0.0f)
    //    {
    //        Destroy(gameObject);
    //        Debug.Log("See you next time!");
    //    }
    //}

    // Check if landed on train
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == landingCollider)
        {
            transform.parent = landingCollider.gameObject.transform;
            mCurrentState = State.OnTrain;
        }
    }

    // Check if landed on ground
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //if (collision.gameObject == groundArea)
    //{
    //mCurrentState = State.OnGround;
    //Destroy(gameObject);
    //}
    //}

    // Check if outside of screen 
    private void OffScreen()
    {

        if (transform.position.x < leftRange)
        // || transform.position.x >rightRange )
        {
            mCurrentState = State.Nothing;
            Destroy(gameObject);
        }
    }

    // GetTargetPosition 
    private void GetTargetPosition()
    {
        float distance = float.MaxValue;
        targetPos = (transform.position);
        currentTarget = null;

        foreach (var target in targetList)
        {
            if (target != null)
            {

                if (target.GetComponent<TurretHealth>()?.IsAlive == true || (target.GetComponent<TurretHealth>() == null))// && target.GetComponentInParent<Train>()?.IsAlive() == true))
                {

                    if (Vector2.Distance(transform.position, target.transform.position) < distance)
                    {
                        targetPos = (target.transform.position);
                        distance = Vector2.Distance(transform.position, target.transform.position);
                        currentTarget = target;
                    }
                }

            }
        }

        if (currentTarget == null)
        {
            mCurrentState = State.Nothing;
            Debug.Log("Out of target.");
        }
    }
    public bool OutsideScreen()
    {
        return currentPos.y <= screenBottom;
    }
}
