using UnityEngine;

public class EMPShockWave : MonoBehaviour
{
    [SerializeField] private LayerMask _layerEnemyMask = LayerMask.NameToLayer("Default");
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
            _circleCollider.radius += Time.deltaTime * 12.0f;
            _circleCollider.radius = Mathf.Clamp(_circleCollider.radius, 0.0f, 12.0f);

            Debug.Log($"<color=lime> Radius: {_circleCollider.radius}</color>");
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
