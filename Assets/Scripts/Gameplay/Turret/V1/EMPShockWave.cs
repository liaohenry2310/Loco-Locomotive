using System.Collections;
using UnityEngine;

namespace Turret
{

    public class EMPShockWave : MonoBehaviour
    {
        [SerializeField] private TurretData _turretData = null;
        private ObjectPoolManager _objectPoolManager = null;
        private WaitForSeconds _waitSeconds;
        private Vector3 _screenBounds;

        void Start()
        {
            _screenBounds = GameManager.GetScreenBounds;
            if (!_objectPoolManager)
            {
                _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
            }
            transform.localScale = new Vector3(0.0f, 0.0f,0.0f);
            _waitSeconds = new WaitForSeconds(0.05f);
        }

        private void OnEnable()
        {
            StartCoroutine(IncreaseWave());
        }

        private void OnDisable()
        {
            StopCoroutine(IncreaseWave());
        }

        private void FixedUpdate()
        {
            transform.position += transform.up * (_turretData.empShockWave.moveSpeed * Time.fixedDeltaTime);
           
            // set activated false prefabs when touch the camera bounds
            if ((transform.position.x >= _screenBounds.x) ||
                (transform.position.x <= -_screenBounds.x) ||
                (transform.position.y >= _screenBounds.y) ||
                (transform.position.y <= -_screenBounds.y))
            {
                RecycleBullet();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyShieldHealth enemyShield = collision.GetComponentInChildren<EnemyShieldHealth>();
            if (enemyShield)
            {
                enemyShield.TakeDamage(500);
            }
        }

        private IEnumerator IncreaseWave()
        {
            while (transform.localScale.x <= _turretData.empShockWave.maxSize)
            {
                transform.localScale += new Vector3(_turretData.empShockWave.maxSize / _turretData.empShockWave.growthDuration * Time.fixedDeltaTime, 0.01f, 0.0f);
                yield return _waitSeconds;
            }
        }

        private void RecycleBullet()
        {
            // reset the original scale before return to pool
            gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            _objectPoolManager.RecycleObject(gameObject);
        }
    }
}
