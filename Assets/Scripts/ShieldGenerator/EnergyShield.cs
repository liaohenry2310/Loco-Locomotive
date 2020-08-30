using UnityEngine;

public class EnergyShield : MonoBehaviour
{
    private SpriteRenderer _spriteEnergyBarrier = null;
    private PolygonCollider2D _polygonCollider2D = null;

    private void Awake()
    {
        if (!TryGetComponent(out _spriteEnergyBarrier))
        {
            Debug.LogWarning($"[EMPShockWave] -- Failed to get the component: {_spriteEnergyBarrier.name}");
        }
        if (!TryGetComponent(out _polygonCollider2D))
        {
            Debug.LogWarning($"[EMPShockWave] -- Failed to get the component: {_polygonCollider2D.name}");
        }
    }

    private void Start()
    {
        _spriteEnergyBarrier.enabled = false;
    }

    public bool IsShieldActivated => _spriteEnergyBarrier.enabled;

    public void ActivateEnergyBarrier(bool activate)
    {
        _spriteEnergyBarrier.enabled = activate;
        _polygonCollider2D.enabled = activate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}