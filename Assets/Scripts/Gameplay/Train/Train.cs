using Turret;
using Interfaces;
using System;
using System.Collections;
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
        public GameObject ExplosionEffect;
        public AudioClip ExplosionAudio;
        public AudioSource TrainRunningAudio;
        public AudioSource LowFuelAudio;

        private bool _audioPlayed = false;
        private bool _isDestroyed = false;
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
                Debug.LogWarning("<color=red>FireBox</color> are not assigned into the Train Script.");
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
                Debug.LogWarning("<color=red>FireBox</color> are not assigned into the Train Script.");
            }
        }

        private void Update()
        {
            // Check how many player has on the scene to increate the FuelRate
            if(LevelManager.Instance.TimeRemaining != LevelManager.Instance.TimeLimit)
                CurrentFuel(_trainData.FuelRate * (_trainData.PlayerCount / 4.0f) * Time.deltaTime);

            if (_trainData.FuelPercentage < 0.3f && !_audioPlayed)
            {
                LowFuelAudio.PlayOneShot(LowFuelAudio.clip);
                _audioPlayed = true;
            }
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
                StartCoroutine(DesuctrionAnimation());
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
                StartCoroutine(DesuctrionAnimation());
            }
            //Train Shakes
            _shakeAmount = (damage > 10.0f) ? 0.1f : UnityEngine.Random.Range(0.05f, 0.08f);

            Vector3 shakepos = UnityEngine.Random.insideUnitSphere * _shakeAmount;
            Vector3 pos = _pos + shakepos;
            pos.y = transform.localPosition.y;
            transform.localPosition = pos;
            _ = StartCoroutine(camera_Shake.Shake(0.03f, 0.03f));
        }

        private IEnumerator DesuctrionAnimation()
        {
            if (_isDestroyed)
                yield break;
            else
                _isDestroyed = true;

            int explosionNum = 5;
            GameObject[] explosions = new GameObject[explosionNum];
            for(int i = 0; i < explosionNum; ++i)
            {
                explosions[i] = Instantiate(ExplosionEffect, gameObject.transform);
            }

            float startTime = Time.unscaledTime;
            float duration = 5.0f;
            float t = 0.0f;

            Vector3 startPos = gameObject.transform.position;
            Vector3 finalPos = new Vector3(-13.0f, startPos.y, startPos.z);

            int currentExplosion = 0;
            float explosionTime = Time.unscaledTime;
            float explosionDelay = 0.25f;

            while (t <= 1.0f)
            {
                t = (Time.unscaledTime - startTime) / duration;
                gameObject.transform.position = Vector3.Lerp(startPos, finalPos, t);

                if(Time.unscaledTime >= explosionTime)
                {
                    explosionTime = Time.unscaledTime + explosionDelay;
                    Bounds trainCollider = gameObject.GetComponent<BoxCollider2D>().bounds;
                    Vector2 randomPosition = new Vector2(
                        UnityEngine.Random.Range(trainCollider.min.x, trainCollider.max.x),
                        UnityEngine.Random.Range(trainCollider.min.y, trainCollider.max.y));
                    explosions[currentExplosion].transform.position = randomPosition;
                    explosions[currentExplosion++].GetComponent<ParticleSystem>().Play();
                    currentExplosion %= explosionNum;
                    LowFuelAudio.PlayOneShot(ExplosionAudio);
                    StartCoroutine(camera_Shake.Shake(0.06f, 0.06f));
                }

                yield return null;
            }
            TrainRunningAudio.Stop();
            OnGameOver?.Invoke();
        }
    }
}