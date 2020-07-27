using UnityEngine;

public class DispenserObject : MonoBehaviour
{
    [SerializeField] private DestroyOnDelay _delayOnDestroy = null;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    public SpriteRenderer Sprite { get { return _spriteRenderer; } }

    public void StartDestructionTimer()
    {
        if (_delayOnDestroy)
        {
            _delayOnDestroy.BeginTimer();
        }
    }
}