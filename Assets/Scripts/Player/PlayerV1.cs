using Interfaces;
using Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerV1 : MonoBehaviour, IDamageable<float>
{
    [SerializeField] private PlayerData _playerData = null;

    public IInteractable Interactable { get; set; } = null;
    public LadderController LadderController { get; set; } = null;

    private PlayerInput _playerInput;
    private Rigidbody2D _rigidBody;
    private Vector2 _axis = Vector2.zero;
    private float _playerHeight;


    // ---- Health ------
    private Visuals.HealthBar _healthBar;
    private HealthSystem _healthSystem;

    // ---- PlayerRespawnPoint
    public Vector2 RespawnPoint { get; set; } = Vector2.zero;

    private void Start()
    {
        Initialized();
    }

    private void FixedUpdate()
    {
        // make movement
        _rigidBody.MovePosition(new Vector2(transform.position.x + (_axis.x * _playerData.Speed * Time.fixedDeltaTime), _rigidBody.position.y));
        if (LadderController)
        {
            _rigidBody.gravityScale = 0.0f;

            if (_axis.y != 0.0f)
            {
                //_rigidBody.MovePosition(new Vector2(_rigidBody.position.x, transform.position.y + (_axis.y * _playerData.Speed * Time.fixedDeltaTime)));
                _rigidBody.MovePosition(new Vector2(LadderController.transform.position.x, transform.position.y + (_axis.y * _playerData.Speed * Time.fixedDeltaTime)));
                //_rigidBody.velocity = new Vector2(0.0f, _axis.y * _speed);
                //transform.position = new Vector2(LadderController.transform.position.x, Mathf.Min(transform.position.y, LadderController.GetLadderTopPosition().y + mPlayerHeight * 0.5f));
                //Vector2 playerUsingLadder =  new Vector2(LadderController.transform.position.x, Mathf.Min(transform.position.y, LadderController.LadderTopPosition.y + _playerHeight * 0.5f));
                //_rigidBody.MovePosition(playerUsingLadder);

            }
            //else
            //{
            //    _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0.0f);
            //}
        }
        else
        {
            _rigidBody.gravityScale = _playerData.Gravity;
        }
    }
   
    public void Initialized()
    {
        if (!TryGetComponent(out _playerInput))
        {
            Debug.LogWarning($"Fail to load Player Input component!.");
        }

        if (!TryGetComponent(out _rigidBody))
        {
            Debug.LogWarning($"Fail to load RigidBody component!.");
        }

        _healthBar = GetComponentInChildren<Visuals.HealthBar>();
        _healthSystem = new HealthSystem(_playerData.MaxHealth);
        _healthBar.SetUp(_healthSystem);
        _healthBar.SetBarVisible(false); // Player start with HealthBar invisible

        _playerHeight = GetComponent<CapsuleCollider2D>().size.y;
    }

    #region Player InputAction 

    public void OnDirectional(InputAction.CallbackContext context)
    {
        _axis = context.ReadValue<Vector2>();
    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ActionsPrimary();
        }
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ActionsSecondary();
        }
    }

    #endregion

    #region Turret InputAction

    public void OnRotate(InputAction.CallbackContext context)
    {
        // For now I will use TurretCannon, 
        // I will change for Turret
        TurretCannon turret = (TurretCannon)Interactable;
        if (turret != null)
        {
            turret.OnRotate(context);
        }
    }

    public void OnDetach(InputAction.CallbackContext context)
    {
        TurretCannon turret = (TurretCannon)Interactable;
        if (turret != null)
        {
            turret.OnDetach(context);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        TurretCannon turret = (TurretCannon)Interactable;
        if (turret != null)
        {
            turret.OnFire(context);
        }
    }

    #endregion

    private void ActionsPrimary()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _playerData.Radius, _playerData.InteractableMask);
        foreach (var collider in colliders)
        {
            IInteractable iter = collider.GetComponent<IInteractable>();
            if (iter != null)
            {
                iter.Interact(this);
                break;
            }
        }

        if (colliders.Length == 0)
        {
            Item item = GetChildCount;
            if(item)
            {
                item.DropItem();
            }
        }
    }

    private void OnDrawGizmos()
    {
        // To check the radius using Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _playerData.Radius);
    }

    private void ActionsSecondary()
    {
        // when press input secondary
    }

    public void SwapActionControlToPlayer(bool isPlayer)
    {
        if (isPlayer)
        {
            _playerInput.SwitchCurrentActionMap("Input");
        }
        else
        {
            _playerInput.SwitchCurrentActionMap("Turret");
        }
    }

    public void TakeDamage(float damage)
    {
        _healthSystem.Damage(damage);
        _healthBar.SetBarVisible(true);
        // --- When Player is dead
        if (_healthSystem.Health < 0.1f)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            StartCoroutine(StartRegeneration());
        }
    }

    private IEnumerator StartRegeneration()
    {
        yield return new WaitForSeconds(3.0f);
        _healthSystem.RestoreHealth(_playerData.MaxHealth);
        yield return new WaitForSeconds(1.0f);
        _healthBar.SetBarVisible(false);
    }

    private IEnumerator Respawn()
    {
        SpriteRenderer localSprite = GetComponent<SpriteRenderer>();
        localSprite.enabled = false;
        _healthBar.SetBarVisible(false);
        yield return new WaitForSeconds(5.0f);
        localSprite.enabled = true;
        _healthBar.SetBarVisible(true);
        transform.position = RespawnPoint;
        _healthSystem.RestoreHealth(_playerData.MaxHealth);
    }
    public Item GetChildCount => GetComponentInChildren<Item>();

}
