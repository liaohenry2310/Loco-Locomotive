using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private float speed = 5.0f;

    private Rigidbody2D rigidBody;
    private float inputHorizontal;
    private float inputVertical;

    public LadderController LadderController { get; set; }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector2(inputHorizontal * speed, rigidBody.velocity.y);
        if (LadderController)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, inputVertical * speed);
            if (inputVertical != 0)
            {
                transform.position = new Vector2(LadderController.transform.position.x, transform.position.y);
            }
            rigidBody.gravityScale = 0f;
        }
        else
        {
            rigidBody.gravityScale = 5f;
        }
    }

}
