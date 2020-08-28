using UnityEngine;

public class EMPShockWave : MonoBehaviour
{
    [SerializeField] private LayerMask _layerEnemyMask = default;

    private ParticleSystem _shockWaveParticle;
    private CircleCollider2D _circleCollider;

    private float _speedRate = 10f;

    private void Awake()
    {
        if (!TryGetComponent(out _shockWaveParticle))
        {
            Debug.LogWarning($"[EMPShockWave] -- Failed to get the component: {_shockWaveParticle.name}");
        }

        if (!TryGetComponent(out _circleCollider))
        {
            Debug.LogWarning($"[EMPShockWave] -- Failed to get the component: {_circleCollider.name}");
        }
        _shockWaveParticle.Stop();
    }

    private void Update()
    {
        if (_shockWaveParticle.isPlaying)
        {
            var main = _shockWaveParticle.main;
            _circleCollider.radius += Mathf.Lerp(_shockWaveParticle.sizeOverLifetime.size.curve.keys[0].value, _shockWaveParticle.sizeOverLifetime.size.curve.keys[2].value, main.simulationSpeed / (_speedRate * 4f));
        }
        else
        {
            _circleCollider.radius = 0f;
        }
    }

    public void PlayShockWave(float speedRate = 1f)
    {
        _speedRate = speedRate;
        _shockWaveParticle.Play();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, _circleCollider.radius, _layerEnemyMask);
        foreach (Collider2D enemy in colliders)
        {
            var enemyShield = enemy.GetComponentInChildren<EnemyShieldHealth>();
            if (enemyShield)
            {
                enemyShield.TakeDamage(500);
            }
        }
    }

}
