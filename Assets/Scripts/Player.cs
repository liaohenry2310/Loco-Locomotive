using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    //Gameobjects that are in contact with the player
    public LadderController LadderController { get; set; }
    public InputReciever InputReceiver { get; set; }

    //Exposed Player properties in the inspector
    [Header("Properties")]
    [SerializeField]
    private float movementSpeed = 3.0f;
    [SerializeField]
    private float gravity = 5.0f;

    //Private members
    private Rigidbody2D mRigidBody;
    private float mPlayerHeight;

    private PlayerInput mPlayerInput;
    private InputAction mActionDirectional;
    private InputAction mActionPrimary;
    private InputAction mActionPrimaryHold;
    private InputAction mActionSeconary;
    private InputAction mActionSeconaryHold;

    private Vector2 mInputMovement;
    private bool mInputPrimary;
    private bool mInputPrimaryHold;
    private bool mInputSecondary;
    private bool mInputSecondaryHold;

    private bool mUsingControllable;

    private void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mPlayerHeight = GetComponent<CapsuleCollider2D>().size.y;

        mPlayerInput = GetComponent<PlayerInput>();
        mActionDirectional = mPlayerInput.actions["Directional"];
        mActionPrimary = mPlayerInput.actions["Primary"];
        mActionPrimaryHold = mPlayerInput.actions["PrimaryHold"];
        mActionSeconary = mPlayerInput.actions["Secondary"];
        mActionSeconaryHold = mPlayerInput.actions["SecondaryHold"];

        mUsingControllable = false;
    }

    private void Update()
    {
        //Update input states from PlayerInput
        mInputMovement = mActionDirectional.ReadValue<Vector2>();
        mInputPrimary = mActionPrimary.triggered;
        mInputPrimaryHold = mActionPrimaryHold.triggered;
        mInputSecondary = mActionSeconary.triggered;
        mInputSecondaryHold = mActionSeconaryHold.triggered;

        //Check when to switch from player controls to controllable controls
        if (!mUsingControllable && InputReceiver && mInputPrimary)
        {
            mRigidBody.velocity = Vector2.zero;
            mUsingControllable = true;
        }
        else if (mUsingControllable && InputReceiver && mInputPrimary)
        {
            mUsingControllable = false;
            InputReceiver.SetZeroInput();
        }

        //Pass input data to the controllable that the player is interacting with
        if (mUsingControllable)
        {
            InputReceiver.SetInput(mInputMovement, mInputPrimary, mInputPrimaryHold, mInputSecondary, mInputSecondaryHold);
        }
    }

    private void FixedUpdate()
    {
        //If not using a controllable, control player movement
        if (!mUsingControllable)
        {
            mRigidBody.velocity = new Vector2(mInputMovement.x * movementSpeed, mRigidBody.velocity.y);

            if (LadderController)
            {
                mRigidBody.gravityScale = 0.0f;

                if (mInputMovement.y != 0.0f)
                {
                    mRigidBody.velocity = new Vector2(0.0f, mInputMovement.y * movementSpeed);
                    transform.position = new Vector2(LadderController.transform.position.x, Mathf.Min(transform.position.y, LadderController.GetLadderTopPosition().y + mPlayerHeight * 0.5f));
                }
                else
                {
                    mRigidBody.velocity = new Vector2(mRigidBody.velocity.x, 0.0f);
                }
            }
            else
            {
                mRigidBody.gravityScale = gravity;
            }
        }
    }
}
