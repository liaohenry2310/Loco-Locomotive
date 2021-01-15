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
        [SerializeField] private SpriteRenderer _spriteCannon = null;

        private Color _defaultColor;
        private readonly WaitForSeconds _waitForSecondsDamage = new WaitForSeconds(0.05f);
        private Vector3 _spriteOriginalPos;
        private Vector3 _spriteCannonOriginalPos;

        public HealthSystem HealthSystem { get; private set; }

        private void Awake()
        {
            HealthSystem = new HealthSystem(_turretData.MaxHealth);
            _defaultColor = _spriteDamageIndicator.color;
            _spriteOriginalPos = _spriteDamageIndicator.gameObject.transform.localPosition;
            _spriteCannonOriginalPos = _spriteCannon.gameObject.transform.localPosition;
        }

        public bool IsAlive => HealthSystem.IsAlive;

        public void TakeDamage(float damage)
        {
            HealthSystem.Damage(damage);
            StartCoroutine(DamageIndicator());
            StartCoroutine(ShakeTurret());
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


        private IEnumerator ShakeTurret()
        {
            float time = 0f;
            while (time <= _turretData.ShakeTime)
            {
                time += Time.deltaTime;
                float ammountToShake = UnityEngine.Random.insideUnitSphere.x * _turretData.ShakeForce;
                _spriteDamageIndicator.gameObject.transform.localPosition = new Vector3(_spriteOriginalPos.x + ammountToShake, 0.0f, 0.0f);
                _spriteCannon.gameObject.transform.localPosition = new Vector3(_spriteCannonOriginalPos.x + ammountToShake, _spriteCannonOriginalPos.y, 0.0f); ;
                yield return null;
            }
            _spriteDamageIndicator.gameObject.transform.localPosition = _spriteOriginalPos;
            _spriteCannon.gameObject.transform.localPosition = _spriteCannonOriginalPos;
        }

    }

}