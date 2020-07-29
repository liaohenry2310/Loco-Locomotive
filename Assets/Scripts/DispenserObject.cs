using UnityEngine;

public class DispenserObject : MonoBehaviour
{
    [SerializeField] private DestroyOnDelay _delayOnDestroy = null;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    public SpriteRenderer Sprite { get { return _spriteRenderer; } }
    public TurretRepair turretRepair;
    public TurretLoader turretLoader;
    public FireBox fireBox;

    public void StartDestructionTimer()
    {
        if (_delayOnDestroy)
        {
            _delayOnDestroy.BeginTimer();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (!player)
            {
                Debug.LogError($"Item failed to find player");
                return;
            }
            player.dispenserObject = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (!player)
            {
                Debug.LogError($"Item failed to find player");
                return;
            }
            player.dispenserObject = null;


        }
    }

    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}