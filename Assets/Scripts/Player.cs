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

    private GameObject _itemDispenserSprite = default;
    public bool PlayerHasItem { get; private set; } = false;
    private SpriteRenderer _spriteRender;
    private DispenserItem _currentDispenser;
    public DispenserData.Type DispenserDataType;

    #endregion

    //public GameObject ammoSprite;
    //public GameObject repairKitSprite;
    //public GameObject fuelSprite;
    //public GameObject playerSprite;
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

        #region Dispenser

        if (mInputReceiver.GetSecondaryInput())
        {
            if (DispenserDataType != DispenserData.Type.None)
            {
                PlayerHasItem = true;
                _spriteRender.color = _currentDispenser.DispenserColor;
                DispenserDataType = _currentDispenser.DispenserType;
                _itemDispenserSprite.SetActive(true);
                Debug.Log($"Player has item? {PlayerHasItem} --- {DispenserDataType}");

                //if (PlayerHasItem)
                //{
                PickUpAmmo();
                PickUpFuel();
                PickUpRepairKit();
                //}
            }
            else
            {
                PlayerHasItem = false;
                _itemDispenserSprite.SetActive(false);
                Debug.Log($"Player has item? {PlayerHasItem} --- {DispenserDataType}");
            }
        }

        #endregion
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
            //ammoSprite.SetActive(false);
            //repairKitSprite.SetActive(false);
            //fuelSprite.SetActive(false);
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
        PlayerHasItem = false;
        _itemDispenserSprite.SetActive(false);
        DispenserDataType = DispenserData.Type.None;
    }

    private void PickUpFuel()
    {
        if (fireBox)
        {
            if (DispenserDataType == DispenserData.Type.Fuel)
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
            if (DispenserDataType == DispenserData.Type.RepairKit)
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
            switch (DispenserDataType)
            {
                case DispenserData.Type.LaserBeam:
                case DispenserData.Type.Missile:
                case DispenserData.Type.Railgun:
                case DispenserData.Type.Normal:
                    {
                        turretLoader.Reloadammo(DispenserDataType);
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

    public void SetCurrentDispenser(DispenserItem dispenser)
    {
        if (dispenser == null)
        {
            Debug.Log("Clearing the current dispenser type");

            if (!PlayerHasItem)
            {
                DispenserDataType = DispenserData.Type.None;
            }
        }
        else
        {
            if (!PlayerHasItem)
            {
                _currentDispenser = dispenser;
                DispenserDataType = _currentDispenser.DispenserType;
            }
        }
    }

}