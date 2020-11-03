using Manager;
using UnityEngine;

namespace Visuals
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject _bar = null;

        private HealthSystem _healthSystem;
        private SpriteRenderer _backgroundSprite;

        private void Awake()
        {
            _backgroundSprite = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnDisable()
        {
            _healthSystem.OnUpdateHealthUI -= UpdateHealthUI;
        }

        /// <summary>
        /// Prepare the Visual HealthBar.
        /// Using HealthSystem as callback after register
        /// </summary>
        /// <param name="healthSystem">HealthSystem to register events</param>
        public void SetUp(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            _healthSystem.OnUpdateHealthUI += UpdateHealthUI;
        }

        private void UpdateHealthUI()
        {
            _bar.transform.localScale = new Vector3(_healthSystem.HealthPercentage, 1.0f, 0.0f);
        }

        public void SetBarVisible(bool enable)
        {
            SpriteRenderer barSprite = _bar.GetComponentInChildren<SpriteRenderer>();
            barSprite.enabled = enable;
            _backgroundSprite.enabled = enable;
        }
    }

}