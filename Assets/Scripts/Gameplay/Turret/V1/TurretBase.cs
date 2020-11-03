using Interfaces;
using Items;
using Manager;
using UnityEngine;

namespace Turret
{
    public class TurretBase : MonoBehaviour, IDamageable<float>, IInteractable
    {
        [SerializeField] private TurretData _turretData = null;
        private HealthSystem _healthSystem;

        private void Awake()
        {
            _healthSystem = new HealthSystem(_turretData.MaxHealth);
        }

        public bool IsAlive => _healthSystem.IsAlive;

        public void TakeDamage(float damage)
        {
            _healthSystem.Damage(damage);
        }

        public void Interact(PlayerV1 player)
        {
            Item item = player.GetItem;
            if (item != null && item.ItemType == DispenserData.Type.RepairKit)
            {
                _healthSystem.RestoreFullHealth();
                player.GetItem.DestroyAfterUse();
                Debug.Log($"[TurretBase] -- Repair complete.");
            }
        }
    }

}