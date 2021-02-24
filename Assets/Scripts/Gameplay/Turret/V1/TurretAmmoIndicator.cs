using UnityEngine;

namespace Turret
{
    public class TurretAmmoIndicator : MonoBehaviour
    {
        [SerializeField] private Transform _bar = null;
        private SpriteRenderer _spriteRenderer = null;
        private SpriteRenderer _barSpriteRenderer = null;
        private Color _defaultColorBar;

        private void Awake()
        {
            _bar.localScale = Vector3.one;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // get the first Sprite Rendere they found.

            _barSpriteRenderer = _bar.GetComponentInChildren<SpriteRenderer>();
            if (_barSpriteRenderer)
            {
                _defaultColorBar = _barSpriteRenderer.color;
            }
        }

        public void UpadteIndicator(ref Weapons weapons)
        {
            float valuePerc = weapons.CurretAmmo / weapons.MaxAmmo;
            _bar.localScale = new Vector3(1.0f, valuePerc, 1.0f);
            _spriteRenderer.color = (valuePerc == 0.0f) ? Color.red : Color.grey;
        }

        public void EnableIndicator(bool hidden)
        {
            gameObject.SetActive(hidden);
        }

        public void PlayerUsingTurret(bool isUsing)
        {
            _barSpriteRenderer.color = isUsing ? _defaultColorBar : Color.grey;
        }

    }

}