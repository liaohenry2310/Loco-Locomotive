using Interfaces;
using Items;
using Manager;
using System;
using UnityEngine;

namespace Turret
{
    public class TurretBase : MonoBehaviour, IDamageable<float>, IInteractable
    {
        public event Action<float> OnTakeDamageUpdate;
        [SerializeField] private TurretData _turretData = null;

        public HealthSystem HealthSystem { get; private set; }

        private void Awake()
        {
            HealthSystem = new HealthSystem(_turretData.MaxHealth);
        }

        public bool IsAlive => HealthSystem.IsAlive;

        public void TakeDamage(float damage)
        {
            HealthSystem.Damage(damage);
            OnTakeDamageUpdate?.Invoke(HealthSystem.HealthPercentage);
        }

        public void Interact(PlayerV1 player)
        {
            Item item = player.GetItem;
            if (item != null && item.ItemType == DispenserData.Type.RepairKit)
            {
                HealthSystem.RestoreFullHealth();
                player.GetItem.DestroyAfterUse();
            }
        }

    }

}