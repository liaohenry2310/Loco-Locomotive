using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Machinaries

    public TurretRepair turretRepair;
    public TurretLoader turretLoader;
    public FireBox fireBox;
    public DispenserObject dispenserObject;
    #endregion

    #region dispenser
    private GameObject _itemDispenserSprite = default; 
    public bool PlayerHasItem { get; private set; } = false;
    private SpriteRenderer _spriteRender;
    private DispenserItem _currentItem;         // Item that the player is currently holding.
    private DispenserItemData _itemToPickup;        // The type of dispenser the player is standing at if any.    
    #endregion

    public GameObject player;
    public GameObject spwanPoint;

    public LadderController LadderController { get; set; }
    public PlayerController PlayerController { get; set; }

    public Vector3 itemOffset;

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
        _itemToPickup = new DispenserItemData()
        {
            itemColor = Color.white,
            itemType = DispenserData.Type.None
        };

        _currentItem = new DispenserItem()
        {
            DispenserType = DispenserData.Type.None,
            DispenserColor = Color.white,
        };

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

        if (mInputReceiver.GetSecondaryInput()) 
        {
            // If we are at a dispenser
            if (_itemToPickup.itemType != DispenserData.Type.None)
            {
                if (PlayerHasItem)
                {                   
                    DropItem();// Drop it.
                }
                else
                {                  
                    PickUpItemFromDispenser(); // Pick up
                }
            }
            else
            {
                if (PlayerHasItem)
                {
                    // If we can use the item, ie Refeul, Repair, Reload ... 
                    // Drop it, if we haven't used it.
                    DropItem();                   
                }
                else
                {
                    //Player is not at a dispenser, and is not holding an item ... nothing to do.
                    _itemDispenserSprite.SetActive(false);
                }
            }

            if(dispenserObject !=null)
            {
                _itemDispenserSprite.SetActive(true);
                _spriteRender.color = dispenserObject.Sprite.color;             
                PlayerHasItem = true;
                dispenserObject.OnBecameInvisible();
            }
        }

        #endregion
    }

    private void PickUpItemFromDispenser()
    {
        PlayerHasItem = true;
        _currentItem.DispenserType = _itemToPickup.itemType;
        _currentItem.DispenserColor = _itemToPickup.itemColor;
        _currentItem.ItemPrefab = _itemToPickup.itemPrefab;
        _itemDispenserSprite.SetActive(true);
        _spriteRender.color = _currentItem.DispenserColor;
        Debug.Log($"Player picked up item --- Type: {_currentItem.DispenserType} Color: {_currentItem.DispenserColor.ToString()}");
    }

    private void DropItem()
    {
        if(!fireBox && !turretRepair && !turretLoader)
        {
            // Place item on the ground.
            var itemDropped = GameObject.Instantiate(_currentItem.ItemPrefab, transform.position - itemOffset, Quaternion.identity); 
            var dispenserObject = itemDropped.GetComponent<DispenserObject>();
            if (dispenserObject != null)
            { 
                dispenserObject.Sprite.color = _currentItem.DispenserColor;
                dispenserObject.StartDestructionTimer();
               
            }

            _spriteRender.color = Color.white;
            _itemDispenserSprite.SetActive(false);
            PlayerHasItem = false;
        }
        else
        {
            UseItem();
        }
    }

    private void UseItem()
    {
        ReloadAmmo();
        AddFuel();
        RepairTurret();
    }


    private void DisableHoldItem()
    {
        _spriteRender.color = Color.white;
        _itemDispenserSprite.SetActive(false);
        PlayerHasItem = false;
    }

    private void AddFuel()
    {
        if(fireBox)
        {
            if (_currentItem.DispenserType == DispenserData.Type.Fuel)
            {
                fireBox.AddFuel();
                DisableHoldItem();
            }
        }

    }

    private void RepairTurret()
    {
        if (turretRepair)
        {
            if (_currentItem.DispenserType == DispenserData.Type.RepairKit)
            {
                turretRepair.Repair();
                DisableHoldItem();
            }

        }
    }

    private void ReloadAmmo()
    {
        if (turretLoader)
        {
            switch (_currentItem.DispenserType)
            {
                case DispenserData.Type.LaserBeam:
                case DispenserData.Type.Missile:
                case DispenserData.Type.Railgun:
                case DispenserData.Type.Normal: // MachineGun
                    {
                        turretLoader.Reloadammo(_itemToPickup.itemType);
                        DisableHoldItem();
                    }
                    break;
                case DispenserData.Type.None:
                default:
                    {
                       //nothing to do
                    }
                    break;
            }
        }
    }

    // When we enter/exit the trigger box of the dispenser set the corresponding variable to tell which dispenser we are at.
    public void SetCurrentDispenser(DispenserItem item)
    {
        Debug.Log($"Setting Current Dispenser to {item?.DispenserType}");
        // If the dispenser is null we have exited the trigger box of the dispenser we were just at.
        if (item == null)
        {
            Debug.Log("Clearing the current dispenser type");
            _itemToPickup.itemType = DispenserData.Type.None;
            _itemToPickup.itemColor = Color.white;
            _itemToPickup.itemPrefab = null;
        }
        else
        {
            _itemToPickup.itemType = item.DispenserType;
            _itemToPickup.itemColor = item.DispenserColor;
            _itemToPickup.itemPrefab = item.ItemPrefab;
        }
    }

    #region Player Respawn
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
    #endregion
}