using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private float speed = 4.0f;
    [SerializeField]
    private float gravity = 5.0f;

    private Rigidbody2D rigidBody;
    private float playerHeight;
    private float inputVertical;

    public LadderController LadderController { get; set; }
    private InputManager imputManager;

    private void Start()
    {
        imputManager = FindObjectOfType<InputManager>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerHeight = GetComponent<CapsuleCollider2D>().size.y;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void FixedUpdate()
    {
        LadderMovement();
    }

    private void PlayerMovement()
    {
        if (imputManager.InputCommand == InputManager.EInputCommand.Player)
        {
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");
            rigidBody.velocity = new Vector2(inputHorizontal * speed, rigidBody.velocity.y);
        }
    }

    private void LadderMovement()
    {
        if (imputManager.InputCommand == InputManager.EInputCommand.Player)
        {
            if (LadderController)
            {
                rigidBody.gravityScale = 0.0f;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, inputVertical * speed);
                if (inputVertical != 0.0f)
                {
                    rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
                    transform.position = new Vector2(LadderController.transform.position.x, Mathf.Min(transform.position.y, LadderController.GetLadderTopPosition().y + playerHeight * 0.5f));
                }
                else
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0.0f);
                }
            }
            else
            {
                rigidBody.gravityScale = gravity;
            }
        }
    }

}
