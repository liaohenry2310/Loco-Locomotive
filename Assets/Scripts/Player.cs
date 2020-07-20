using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public AmmoCrate ammoCrate;
    public Repairkitcrate repairkitcrate;
    public FuelCrate fuelCrate;
    public TurretRepair turretRepair;
    public TurretLoader turretLoader;
    public FireBox fireBox;
    public bool isHoldingAmmo;
    public bool isHoldingRepairKit;
    public bool isHoldingFuel;

    #region dispenser

    public GameObject _itemDispenserSprite = default;
    public bool isPlayerInsideDispenser = false;
    public bool PlayerHasItem { get; private set; } = false;
    private SpriteRenderer _spriteRender;
    private DispenserData.Type _dispenserDataType;
    private DispenserItem _currentDispenser;

    #endregion

    public GameObject ammoSprite;
    public GameObject repairKitSprite;
    public GameObject fuelSprite;
    public GameObject playerSprite;
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

        //ammoSprite.SetActive(false);
        //repairKitSprite.SetActive(false);
        //fuelSprite.SetActive(false);
    }

    private void Update()
    {
        ////ammo
        //if (ammoCrate != null && mInputReceiver.GetSecondaryInput())
        //{
        //    isHoldingAmmo = true;
        //    ammoSprite.SetActive(true);
        //}
        //else if (turretLoader != null && isHoldingAmmo && mInputReceiver.GetSecondaryInput())
        //{
        //    isHoldingAmmo = false;
        //    ammoSprite.SetActive(false);
        //    turretLoader.Reloadammo();
        //}
        //else if (isHoldingAmmo && mInputReceiver.GetSecondaryInput())
        //{
        //    ammoSprite.SetActive(false);
        //    isHoldingAmmo = false;
        //}


        ////repairkit
        //if (repairkitcrate != null && mInputReceiver.GetSecondaryInput())
        //{
        //    isHoldingRepairKit = true;
        //    repairKitSprite.SetActive(true);
        //}
        //else if (turretRepair != null && isHoldingRepairKit && mInputReceiver.GetSecondaryInput())
        //{
        //    isHoldingRepairKit = false;
        //    repairKitSprite.SetActive(false);
        //    turretRepair.Repair();

        //}
        //else if (isHoldingRepairKit && mInputReceiver.GetSecondaryInput())
        //{
        //    repairKitSprite.SetActive(false);
        //    isHoldingRepairKit = false;
        //}

        ////fuel
        //if (fuelCrate != null && mInputReceiver.GetSecondaryInput())
        //{
        //    isHoldingFuel = true;
        //    fuelSprite.SetActive(true);
        //}
        //else if (fireBox != null && isHoldingFuel && mInputReceiver.GetSecondaryInput())
        //{
        //    isHoldingFuel = false;
        //    fuelSprite.SetActive(false);
        //    fireBox.AddFuel();
        //}
        //else if (isHoldingFuel && mInputReceiver.GetSecondaryInput())
        //{
        //    fuelSprite.SetActive(false);
        //    isHoldingFuel = false;
        //}

        // Dispenser

        //switch (_dispenserDataType)
        //{
        //    case DispenserData.Type.LaserBeam:
        //    case DispenserData.Type.Missile:
        //    case DispenserData.Type.Railgun:
        //    case DispenserData.Type.Normal:
        //        {
        //            _isPlayerHasItem = true;
        //            _itemDispenserSprite.SetActive(true);
        //        }
        //        break;
        //    case DispenserData.Type.RepairKit:
        //        break;
        //    case DispenserData.Type.Fuel:
        //        break;
        //    case DispenserData.Type.None:
        //        {
        //            _isPlayerHasItem = false;
        //            _itemDispenserSprite.SetActive(false);
        //        }
        //        break;
        //    default:
        //        {
        //            _isPlayerHasItem = false;
        //            _itemDispenserSprite.SetActive(false);
        //        }
        //        break;
        //}


        if (mInputReceiver.GetSecondaryInput())
        {
            if (_currentDispenser.DispenserType != DispenserData.Type.None)
            {
                PlayerHasItem = true;
                _spriteRender.color = _currentDispenser.DispenserColor;
                _itemDispenserSprite.SetActive(true);
                Debug.Log($"Player has item? {PlayerHasItem} --- {_currentDispenser.DispenserType}");
            }
            else
            {
                PlayerHasItem = false;
                _itemDispenserSprite.SetActive(false);
                Debug.Log($"Player has item? {PlayerHasItem} --- {_currentDispenser.DispenserType}");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("player died");
            player.SetActive(false);
            ammoSprite.SetActive(false);
            repairKitSprite.SetActive(false);
            fuelSprite.SetActive(false);
            player.transform.localPosition = spwanPoint.transform.localPosition;
            Invoke("Respawn", 5f);
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawn");
        player.SetActive(true);
    }

    // Dispenser Collection Actions
    public void PickUpFuel(Color itemColor)
    {
        isHoldingFuel = true;
        _spriteRender.color = itemColor;
        _itemDispenserSprite.SetActive(true);
    }

    public void PickUpRepairKit(Color itemColor)
    {
        isHoldingRepairKit = true;
        _spriteRender.color = itemColor;
        _itemDispenserSprite.SetActive(true);
    }

    public void PickUpAmmo(Color itemColor)
    {
        isHoldingAmmo = true;
        _spriteRender.color = itemColor;
        _itemDispenserSprite.SetActive(true);
    }

    public void SetCurrentDispenser(DispenserItem dispenser)
    {
        if (dispenser == null)
        {
            Debug.Log("Clearing the current dispenser type");
            //_itemDispenserSprite.SetActive(false);
            _currentDispenser.DispenserType = DispenserData.Type.None;
        }
        else
        {
            Debug.Log($"Setting current dispenser type to {dispenser.DispenserType.ToString()}");
            _currentDispenser = dispenser;
        }
    }

    public void PickUpItem(bool isInsideDispenser, DispenserData.Type dataType, Color color)
    {
        isPlayerInsideDispenser = isInsideDispenser;
        if (isInsideDispenser && dataType != _dispenserDataType)
        {
            //_currentDispenserDataType = dataType;
            _dispenserDataType = dataType;
            _spriteRender.color = color;
            Debug.Log($"[PickUpItem] { _dispenserDataType} and {_spriteRender.color}");
            //if (dataType != _dispenserDataType)
            //{
            //    _dispenserDataType = dataType;
            //    _spriteRender.color = color;
            //}
        }
        else
        {
            _dispenserDataType = DispenserData.Type.None;
        }

        //if ((dataType != _dispenserDataType))
        //{
        //    _dispenserDataType = dataType;
        //    //_isPlayerHasItem = hasItem;
        //    Debug.Log($"{_dispenserDataType}");
        //    if (_isPlayerHasItem)
        //    {
        //        _spriteRender.color = color;
        //    }
        //}
    }

    // TODO - Cyro - Add dispenser related functions
}