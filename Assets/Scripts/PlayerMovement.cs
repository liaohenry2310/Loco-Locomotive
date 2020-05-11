using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private float distance;

    private readonly float Speed = 10.0f;
    private Rigidbody2D rigidBody;
    private float inputHorizontal;
    private float inputVertical;
    public LayerMask whatIsLadder;
    private bool isClimbing = false;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector2(inputHorizontal * Speed, rigidBody.velocity.y);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

        if (hitInfo.collider)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing = true;
            }
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow)) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                isClimbing = false;
            }
        }

        if (isClimbing && hitInfo.collider)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, inputVertical * Speed);
            rigidBody.gravityScale = 0f;
        }
        else
        {
            rigidBody.gravityScale = 5f;
        }

    }
}
