﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public SpriteRenderer itemSprite; // replace the ones under this.
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


    #region DispenserAmmoType

    private DispenserController mDispenserController;

    #endregion


    private void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mInputReceiver = GetComponent<InputReciever>();
        mPlayerHeight = GetComponent<CapsuleCollider2D>().size.y;
        ammoSprite.SetActive(false);
        repairKitSprite.SetActive(false);
        fuelSprite.SetActive(false);

        // Dispenser
        mDispenserController = GetComponent<DispenserController>();
    }

    private void Update()
    {
        //ammo
        if (ammoCrate != null && mInputReceiver.GetSecondaryInput())
        {
            isHoldingAmmo = true;
            ammoSprite.SetActive(true);
        }
        else if (turretLoader != null && isHoldingAmmo && mInputReceiver.GetSecondaryInput())
        {
            isHoldingAmmo = false;
            ammoSprite.SetActive(false);
            turretLoader.Reloadammo();
        }
        else if (isHoldingAmmo && mInputReceiver.GetSecondaryInput())
        {
            ammoSprite.SetActive(false);
            isHoldingAmmo = false;
        }


        //repairkit
        if (repairkitcrate != null && mInputReceiver.GetSecondaryInput())
        {
            isHoldingRepairKit = true;
            repairKitSprite.SetActive(true);
        }
        else if (turretRepair != null && isHoldingRepairKit && mInputReceiver.GetSecondaryInput())
        {
            isHoldingRepairKit = false;
            repairKitSprite.SetActive(false);
            turretRepair.Repair();

        }
        else if (isHoldingRepairKit && mInputReceiver.GetSecondaryInput())
        {
            repairKitSprite.SetActive(false);
            isHoldingRepairKit = false;
        }

        //fuel
        if (fuelCrate != null && mInputReceiver.GetSecondaryInput())
        {
            isHoldingFuel = true;
            fuelSprite.SetActive(true);
        }
        else if (fireBox != null && isHoldingFuel && mInputReceiver.GetSecondaryInput())
        {
            isHoldingFuel = false;
            fuelSprite.SetActive(false);
            fireBox.AddFuel();
        }
        else if (isHoldingFuel && mInputReceiver.GetSecondaryInput())
        {
            fuelSprite.SetActive(false);
            isHoldingFuel = false;
        }

        // Dispenser

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
        itemSprite.color = itemColor;
    }

    public void PickUpRepairKit(Color itemColor) 
    {
        isHoldingRepairKit = true;
        itemSprite.color = itemColor;
    }

    // TODO - Cyro - Add dispenser related functions
}