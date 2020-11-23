using UnityEngine;

namespace Turret
{
    public class ShieldGunController : MonoBehaviour
    {
        private PolygonCollider2D _polygonCollider = null;
        private SpriteRenderer _spriteRenderer = null;
        private ObjectPoolManager _objectPoolManager = null;

        private void Awake()
        {
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
            if (!TryGetComponent(out _polygonCollider))
            {
                Debug.LogWarning("Failed to load PolygonCollider2D component.");
            }
            if (!TryGetComponent(out _spriteRenderer))
            {
                Debug.LogWarning("Failed to load SpriteRenderer component.");
            }
            EnabledShield(false);
        }

        public void EnabledShield(bool enabled)
        {
            _polygonCollider.enabled = enabled;
            _spriteRenderer.enabled = enabled;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                _objectPoolManager.RecycleObject(collider.gameObject);
            }
        }

    }

}