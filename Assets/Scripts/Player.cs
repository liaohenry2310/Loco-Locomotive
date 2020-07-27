using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    #region dont need anymore
    
    public AmmoCrate ammoCrate;
    public Repairkitcrate repairkitcrate;
    public FuelCrate fuelCrate;

    public bool isHoldingAmmo;
    public bool isHoldingRepairKit;
    public bool isHoldingFuel;

    #endregion

    #region Machinaries
    public TurretRepair turretRepair;
    public TurretLoader turretLoader;
    public FireBox fireBox;

    #endregion

    #region dispenser

    private GameObject _itemDispenserSprite = default; // TODO - Christy: Each item we pickup doesn't need a separate sprite in the player prefab. We can probably get rid of this.
    public bool PlayerHasItem { get; private set; } = false;
    private SpriteRenderer _spriteRender;
    private DispenserItem _currentItem;         // Item that the player is currently holding.
    private DispenserItem _itemToPickup;        // The type of dispenser the player is standing at if any.    

    #endregion

    public GameObject player;
    public GameObject spwanPoint;

    public LadderController LadderController { get; set; }
    public PlayerController PlayerController { get; set; }

    [Header("Properties")]
    [SerializeField]
    private float movementSpeed = 3.0f;
    [SerializeField]
    private float gravity = 5.0f;

    private Rigidbody2D mRigidBody;
    private InputReciever mInputReceiver;
    private float mPlayerHeight;


    private void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mInputReceiver = GetComponent<InputReciever>();
        mPlayerHeight = GetComponent<CapsuleCollider2D>().size.y;

        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            if (sprite.name == "CollectedItem")
            {
                _spriteRender = sprite;
                _itemDispenserSprite = sprite.gameObject;
                _itemDispenserSprite.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Mathf.Round(mInputReceiver.GetHorizontalInput() + 0.2f);
        float vertical = Mathf.Round(mInputReceiver.GetVerticalInput() + 0.2f);

        mRigidBody.velocity = new Vector2(horizontal * movementSpeed, mRigidBody.velocity.y);

        if (LadderController)
        {
            mRigidBody.gravityScale = 0.0f;

            if (vertical != 0.0f)
            {
                mRigidBody.velocity = new Vector2(0.0f, vertical * movementSpeed);
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

    private void Update()
    {
        #region Dispenser

        if (mInputReceiver.GetSecondaryInput()) // 'v' key
        {
            // If we are at a dispenser
            if (_itemToPickup.DispenserType != DispenserData.Type.None)
            {
                if (PlayerHasItem)
                {
                    // Drop it.
                    DropItemOnGround();
                }
                else
                {
                    // Pick up
                    PickUpItemFromDispenser();
                }
            }
            else
            {
                if (PlayerHasItem)
                {
                    // TODO - Christy: Check if we can use the item before we drop it.
                    
                    // If we can use the item, ie Refeul, Repair, Reload ... 
                    UseItem();

                    // Drop it, if we haven't used it.
                    if (_currentItem != null)
                    { 
                        DropItemOnGround();
                    }
                }
                else
                {
                    // Player is not at a dispenser, and is not holding an item ... nothing to do.
                }
            }

            if (_itemToPickup.DispenserType != DispenserData.Type.None)
            {
            }
            else
            {
                PlayerHasItem = false;
                _itemDispenserSprite.SetActive(false);
                Debug.Log($"Player has item? {PlayerHasItem} --- {_itemToPickup.DispenserType}");
            }
        }

        #endregion
    }

    private void PickUpItemFromDispenser()
    {
        PlayerHasItem = true;
        _currentItem = _itemToPickup;
        _spriteRender.color = _currentItem.DispenserColor;
        Debug.Log($"Player picked up item --- Type: {_currentItem.DispenserType} Color: {_currentItem.DispenserColor.ToString()}");
    }

    private void DropItemOnGround()
    {
        // Place item on the ground.
        var itemDropped = GameObject.Instantiate(_currentItem.ItemPrefab, transform); // Might need to adjust where it is spawned.
        var dispenserObject = itemDropped.GetComponent<DispenserObject>();
        if (dispenserObject != null)
        { 
            dispenserObject.Sprite.color = _currentItem.DispenserColor;
            dispenserObject.StartDestructionTimer();
        }

        _currentItem = null;
        _spriteRender.color = Color.white;
        PlayerHasItem = false;
    }

    private void UseItem()
    {
        // TODO - Christy: Rename these to reflect what they are doing.
        PickUpAmmo();
        PickUpFuel();
        PickUpRepairKit();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("player died");
            player.SetActive(false);
            _itemDispenserSprite.SetActive(false);
            player.transform.localPosition = spwanPoint.transform.localPosition;
            Invoke("Respawn", 5f);
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawn");
        player.SetActive(true);
    }

    private void DisableHoldItem()
    {
        // TODO - Christy: Set current item to null
        PlayerHasItem = false;
        _itemDispenserSprite.SetActive(false);
    }

    private void PickUpFuel()
    {
        if (fireBox)
        {
            if (_currentItem.DispenserType == DispenserData.Type.Fuel)
            {
                fireBox.AddFuel();
                DisableHoldItem();
            }
            else
            {
                DisableHoldItem();
            }
        }
    }

    private void PickUpRepairKit()
    {
        if (turretRepair)
        {
            if (_currentItem.DispenserType == DispenserData.Type.RepairKit)
            {
                turretRepair.Repair();
                DisableHoldItem();
            }
            else
            {
                DisableHoldItem();
            }
        }
    }

    private void PickUpAmmo()
    {
        if (turretLoader)
        {
            switch (_currentItem.DispenserType)
            {
                case DispenserData.Type.LaserBeam:
                case DispenserData.Type.Missile:
                case DispenserData.Type.Railgun:
                case DispenserData.Type.Normal: // MachineGun?
                    {
                        turretLoader.Reloadammo();
                        DisableHoldItem();
                    }
                    break;
                case DispenserData.Type.None:
                default:
                    {
                        DisableHoldItem();
                    }
                    break;
            }
        }
    }

    // When we enter/exit the trigger box of the dispenser set the corresponding variable to tell which dispenser we are at.
    public void SetCurrentDispenser(DispenserItem item)
    {
        // If the dispenser is null we have exited the trigger box of the dispenser we were just at.
        if (item == null)
        {
            Debug.Log("Clearing the current dispenser type");
            _itemToPickup.DispenserType = DispenserData.Type.None;
            _itemToPickup.DispenserColor = Color.white;
        }
        else
        {
            _itemToPickup = item;
        }
    }
}