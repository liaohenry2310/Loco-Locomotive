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
    public void Destroy()
    {
        if(turretRepair|| turretLoader|| fireBox)
        {
            Destroy(gameObject);
            Debug.Log("Destroy item");
        }
    }
}