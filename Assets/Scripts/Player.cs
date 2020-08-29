using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Machinaries

    public TurretRepair turretRepair;
    public TurretLoader turretLoader;
    public IShieldGenerator shieldGenerator;
    public FireBox fireBox;
    public DispenserObject dispenserObject;
    #endregion

    #region dispenser
    [SerializeField] private GameObject _collectedItemObject = null;

    public bool PlayerHasItem { get; private set; } = false;

    private SpriteRenderer _collectedItemSprite;
    private DispenserItem _currentItem;         // Item that the player is currently holding.
    private DispenserItemData _itemToPickup;        // The type of dispenser the player is standing at if any.    
    #endregion

    public GameObject player;
    //public GameObject spwanPoint;

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
            itemType = DispenserData.Type.None,
            sprite = null
           
        };

        _currentItem = new DispenserItem()
        {
            DispenserType = DispenserData.Type.None,
            DispenserColor = Color.white,
            sprite = null
        };

        dispenserObject = new DispenserObject()
        {
            ObjectType = DispenserData.Type.None,
            ObjectColor = Color.white,
            Objectsprite = null

        };

        mRigidBody = GetComponent<Rigidbody2D>();
        mInputReceiver = GetComponent<InputReciever>();
        mPlayerHeight = GetComponent<CapsuleCollider2D>().size.y;

        _collectedItemSprite = _collectedItemObject.GetComponent<SpriteRenderer>();
        _collectedItemObject.SetActive(false);
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
                    _collectedItemObject.SetActive(false);
                }
            }

            if (dispenserObject != null)
            {

                _currentItem.DispenserType = dispenserObject.ObjectType;
                _currentItem.DispenserColor = dispenserObject.ObjectColor;
                _currentItem.Objectsprite = dispenserObject.Objectsprite;

                _collectedItemObject.SetActive(true);
                PlayerHasItem = true;
                dispenserObject.OnBecameInvisible();
                Debug.Log($"Player repicked up item --- Type: {_currentItem.DispenserType} Color: {_currentItem.DispenserColor.ToString()}");
            }
        }
        if (GetComponent<PlayerHealth>().IsAlive() == false)
        {
            _collectedItemObject.SetActive(false);
            PlayerHasItem = false;
        }

        #endregion
    }

    private void PickUpItemFromDispenser()
    {
        if (dispenserObject == null)
        {
            PlayerHasItem = true;
            _currentItem.DispenserType = _itemToPickup.itemType;
            _currentItem.DispenserColor = _itemToPickup.itemColor;
            _currentItem.Objectsprite = _itemToPickup.Objectsprite;

            _currentItem.ItemPrefab = _itemToPickup.itemPrefab;

            _collectedItemObject.SetActive(true);

            _collectedItemSprite.sprite = _currentItem.Objectsprite;

            Debug.Log($"Player picked up item --- Type: {_currentItem.DispenserType} Color: {_currentItem.DispenserColor}");
        }
    }

    private void DropItem()
    {
        if (!fireBox && !turretRepair && !turretLoader && shieldGenerator == null)
        {
            // Place item on the ground.
            var itemDropped = GameObject.Instantiate(_currentItem.ItemPrefab, transform.position - itemOffset, Quaternion.identity);
            var dispenserObject = itemDropped.GetComponent<DispenserObject>();
            dispenserObject.Sprite.sprite = _collectedItemSprite.sprite;

            if (dispenserObject != null)
            {
                dispenserObject.ObjectType = _currentItem.DispenserType;
                dispenserObject.ObjectColor = _currentItem.DispenserColor;
                dispenserObject.Sprite.color = _currentItem.DispenserColor;

                //dispenserObject.sprite = _collectedItemSprite.sprite;

                dispenserObject.StartDestructionTimer();
                dispenserObject.itemIndicator.gameObject.SetActive(true);
            }

            _collectedItemSprite.color = Color.white;
            _collectedItemObject.SetActive(false);
            PlayerHasItem = false;
            Debug.Log($"Player droped up item --- Type: {_currentItem.DispenserType} Color: {_currentItem.DispenserColor.ToString()}");
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
        _collectedItemSprite.color = Color.white;
        _collectedItemObject.SetActive(false);
        PlayerHasItem = false;
    }

    private void AddFuel()
    {
        if (fireBox)
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
        if (_currentItem.DispenserType == DispenserData.Type.RepairKit)
        {
            if (turretRepair)
            {
                turretRepair.Repair();
                DisableHoldItem();
            }

            if (shieldGenerator != null)
            {
                shieldGenerator.Repair();
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
                        turretLoader.Reloadammo(_currentItem.DispenserType);
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
            _itemToPickup.Objectsprite = null;
        }
        else
        {
            _itemToPickup.itemType = item.DispenserType;
            _itemToPickup.itemColor = item.DispenserColor;
            _itemToPickup.itemPrefab = item.ItemPrefab;
            _itemToPickup.Objectsprite = item.Objectsprite;
        }
    }
}