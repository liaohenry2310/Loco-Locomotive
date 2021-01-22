using Turret;
using Interfaces;
using System;
using UnityEngine;
namespace GamePlay
{

    public class Train : MonoBehaviour, IDamageable<float>
    {
        public event Action<float> OnUpdateHealthUI;  // HealthUI Action
        public event Action<float> OnUpdateFuelUI;    // FuelUI Action
        public event Action<float> OnFuelReloadUI;    // FireBox Action
        public event Action OnGameOver;               // GameManager action
        public Camera_shake camera_Shake; 
        private float _shakeAmount;
        private Vector3 _pos;
        #region Members

        [SerializeField] private TrainData _trainData = null;
        [SerializeField] private FireBox _fireBox = null;

        private bool _outOfFuel = false;
        #endregion

        public void Initialized()
        {
            _trainData.Initialize(this);
            _trainData.ListTurret = GetComponentsInChildren<TurretBase>();
        }

        private void Start()
        {
            Initialized();
            _pos = this.transform.position;
        }

        private void OnEnable()
        {
            if (_fireBox)
            {
                _fireBox.OnReloadFuel += ReloadFuel;
            }
            else
            {
                Debug.LogWarning("FireBox are not assigned into the Train Script.");
            }
        }

        private void OnDisable()
        {
            if (_fireBox)
            {
                _fireBox.OnReloadFuel -= ReloadFuel;
            }
            else
            {
                Debug.LogWarning("FireBox are not assigned into the Train Script.");
            }
        }

        private void Update()
        {
            // Check how many player has on the scene to increate the FuelRate
            CurrentFuel(_trainData.FuelRate * (_trainData.PlayerCount / 4f) * Time.deltaTime);
        }

        private void ReloadFuel()
        {
            _trainData.CurrentFuel = _trainData.MaxFuel;
            OnFuelReloadUI?.Invoke(_trainData.FuelPercentage);
        }

        public void CurrentFuel(float amount)
        {
            if (_outOfFuel) return;

            _trainData.CurrentFuel -= amount;
            _trainData.CurrentFuel = Mathf.Clamp(_trainData.CurrentFuel, 0.0f, _trainData.MaxFuel);
            OnUpdateFuelUI?.Invoke(_trainData.FuelPercentage);
            if (_trainData.CurrentFuel < 0.01f)
            {
                Debug.Log("[FuelController] Game over.");
                OnGameOver?.Invoke();
                _outOfFuel = true;
                return;
            }
        }

        public void TakeDamage(float damage)
        {
            if (_trainData.CurrentHealth > 0.0f)
            {
                _trainData.CurrentHealth -= damage;
                OnUpdateHealthUI?.Invoke(_trainData.HealthPercentage);
            }
            else if (_trainData.CurrentHealth <= 0.0f)
            {
                OnGameOver?.Invoke();
            }
            //Train Shakes
            if (damage >10)
                _shakeAmount = 0.1f;          
            else
                _shakeAmount = UnityEngine.Random.Range(0.05f, 0.08f);
            Vector3 shakepos = UnityEngine.Random.insideUnitSphere * _shakeAmount;
            Vector3 pos = _pos + shakepos;
            pos.y = transform.localPosition.y;
            transform.localPosition = pos;
            StartCoroutine(camera_Shake.Shake(0.03f, 0.03f));
        }
    }
}