using Turret;
using Interfaces;
using Items;
using Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerV1 : MonoBehaviour, IDamageable<float>
{
    [SerializeField] private PlayerData _playerData = null;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer = null;
    public IInteractable Interactable { get; set; } = null;
    public LadderController LadderController { get; set; } = null;

    private PlayerInput _playerInput;
    private Rigidbody2D _rigidBody;
    private Vector2 _axis = Vector2.zero;
    private AudioSource _audioSource = null;

    // ---- Health ------
    private Visuals.HealthBar _healthBar;
    private HealthSystem _healthSystem;
    private bool _isRespawn = false;

    // ---- PlayerRespawnPoint
    public Vector2 RespawnPoint { get; set; } = Vector2.zero;
    //Animator
    public Animator animator;

    public Vector3 PlayerItemPlaceHolder => transform.position + (_playerSpriteRenderer.bounds.center - transform.position);

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
                _rigidBody.MovePosition(new Vector2(_rigidBody.position.x, transform.position.y + (_axis.y * _playerData.Speed * Time.fixedDeltaTime)));
                //_rigidBody.MovePosition(new Vector2(LadderController.transform.position.x, transform.position.y + (_axis.y * _playerData.Speed * Time.fixedDeltaTime)));
                //_rigidBody.velocity = new Vector2(0.0f, _axis.y * _playerData.Speed);
                //transform.position = new Vector2(LadderController.transform.position.x, Mathf.Min(transform.position.y, LadderController.LadderTopPosition.y + _playerHeight * 0.5f));
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
    #region Animator
    private void Update()
    {
        //flip sprites
        _playerSpriteRenderer.flipX = _axis.x > 0f;

        //moving
        if (_axis.x != 0)
        {
            animator.SetBool("IsMoving", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsClimb", false);
            animator.SetBool("UsingTurret", false);

        }
        //not moving
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsIdle", true);
        }
        //climbing
        if (_axis.y != 0.0f)
        {
            animator.SetBool("IsClimb", true);
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("UsingTurret", false);

        }
        //not climbing
        else
        {
            animator.SetBool("IsClimb", false);
        }
        //using Turret
        TurretGuns turret = Interactable as TurretGuns;
        if (turret != null)
        {
            animator.SetBool("UsingTurret", true);
            animator.SetBool("IsClimb", false);
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsHoldItem", false);
        }
        else
        {
            animator.SetBool("UsingTurret", false);
        }
        //player death
        animator.SetBool("IsDeath", !_healthSystem.IsAlive);
    }

    #endregion
    /// <summary>
    /// Initialize all the components and necessary set up 
    /// </summary>
    public void Initialized()
    {
        if (!TryGetComponent(out _playerInput))
        {
            Debug.LogWarning("Fail to load Player Input component!.");
        }

        if (!TryGetComponent(out _rigidBody))
        {
            Debug.LogWarning("Fail to load RigidBody component!.");
        }

        if (!TryGetComponent(out _audioSource))
        {
            Debug.LogWarning("Fail to load Audio Source component!.");
        }

        _healthBar = GetComponentInChildren<Visuals.HealthBar>();
        _healthSystem = new HealthSystem(_playerData.MaxHealth);
        _healthBar.SetUp(_healthSystem);
        _healthBar.SetBarVisible(false); // Player start with HealthBar invisible

        // _playerHeight = GetComponent<CapsuleCollider2D>().size.y;
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
        TurretGuns turret = Interactable as TurretGuns;
        if (turret != null)
        {
            turret.OnRotate(context);
        }
    }

    public void OnDetach(InputAction.CallbackContext context)
    {
        TurretGuns turret = Interactable as TurretGuns;
        if (turret != null)
        {
            turret.OnDetach(context);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        TurretGuns turret = Interactable as TurretGuns;
        if (turret != null)
        {
            turret.OnFire(context);
        }
    }

    #endregion

    private void ActionsPrimary()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(PlayerItemPlaceHolder, _playerData.Radius, _playerData.InteractableMask);
        foreach (var collider in colliders)
        {
            IInteractable iter = collider.GetComponent<IInteractable>();
            if (iter != null)
            {
                iter.Interact(this);
                animator.SetBool("IsHoldItem", true);
                break;
            }
        }
        if (colliders.Length == 0)
        {
            Item item = GetItem;
            if (item)
            {
                animator.SetBool("IsHoldItem", false);
                item.DropItem();
            }
        }
    }

    private void OnDrawGizmos()
    {
        // To check the radius using Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(PlayerItemPlaceHolder, _playerData.Radius);
    }

    private void ActionsSecondary()
    {
        // when press input secondary
    }

    public void SwapActionControlToPlayer(bool isPlayer) => _playerInput.SwitchCurrentActionMap(isPlayer ? "Input" : "Turret");

    public void TakeDamage(float damage)
    {
        if (_isRespawn) return;

        _healthSystem.Damage(damage);
        _healthBar.SetBarVisible(true);
        _audioSource.clip = _playerData.AudiosClips[0];
        _audioSource.Play();
        // --- When Player is dead
        if (_healthSystem.Health < 0.1f)
        {
            _audioSource.clip = _playerData.AudiosClips[1];
            _audioSource.Play();
            Item item = GetItem;
            item.DestroyAfterUse();
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
        _isRespawn = true;
        SpriteRenderer localSprite = GetComponent<SpriteRenderer>();
        localSprite.enabled = false;
        _healthBar.SetBarVisible(false);
        yield return new WaitForSeconds(5.0f);
        localSprite.enabled = true;
        _healthBar.SetBarVisible(true);
        transform.position = RespawnPoint;
        _healthSystem.RestoreHealth(_playerData.MaxHealth);
        _isRespawn = false;
    }

    public Item GetItem => GetComponentInChildren<Item>();

}
