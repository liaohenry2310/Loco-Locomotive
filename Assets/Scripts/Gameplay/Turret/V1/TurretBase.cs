using Interfaces;
using Items;
using Manager;
using System;
using System.Collections;
using UnityEngine;

namespace Turret
{
    public class TurretBase : MonoBehaviour, IDamageable<float>, IInteractable
    {
        public event Action<float> OnTakeDamageUpdate;
        public event Action OnRepairUpdate;
        [SerializeField] private TurretData _turretData = null;
        [SerializeField] private SpriteRenderer _spriteDamageIndicator = null;
        private Color _defaultColor;
        private readonly WaitForSeconds _waitForSecondsDamage = new WaitForSeconds(0.05f);

        public HealthSystem HealthSystem { get; private set; }

        private void Awake()
        {
            HealthSystem = new HealthSystem(_turretData.MaxHealth);
            _defaultColor = _spriteDamageIndicator.color;
        }

        public bool IsAlive => HealthSystem.IsAlive;

        public void TakeDamage(float damage)
        {
            HealthSystem.Damage(damage);
            StartCoroutine(DamageIndicator());
            OnTakeDamageUpdate?.Invoke(HealthSystem.HealthPercentage);
        }

        public void Interact(PlayerV1 player)
        {
            Item item = player.GetItem;
            if (item != null && item.ItemType == DispenserData.Type.RepairKit)
            {
                HealthSystem.RestoreFullHealth();
                player.GetItem.DestroyAfterUse();
                OnRepairUpdate?.Invoke();
            }
        }

        private IEnumerator DamageIndicator()
        {
            _spriteDamageIndicator.color = Color.red; 
            yield return _waitForSecondsDamage;
            _spriteDamageIndicator.color = _defaultColor;
        }

    }

}