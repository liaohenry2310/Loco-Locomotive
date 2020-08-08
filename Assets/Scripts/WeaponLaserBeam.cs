using UnityEngine;

public class WeaponLaserBeam : MonoBehaviour
{
    [SerializeField] private Transform _cannonFirePoint = default;
    [SerializeField] private LaserData _laserData = default;
    [SerializeField] private LineRenderer _LaserBeam = default;

    [Header("Ammo capacity")]
    public float MaxAmmo = 100;

    public float CurrentAmmo { get; private set; } = 0.0f;

    private void Start() => CurrentAmmo = MaxAmmo;

    public void SetFire(bool isTrigger)
    {
        if (isTrigger && CurrentAmmo > 0f)
        {
            if (!_LaserBeam.enabled)
            {
                _LaserBeam.enabled = true;
            }
            RaycastHit2D hit = Physics2D.Raycast(_cannonFirePoint.transform.position, _cannonFirePoint.transform.up, _laserData.Range);
            _LaserBeam.SetPosition(0, _cannonFirePoint.transform.position);
            //_LaserBeam.SetPosition(1, _cannonFirePoint.transform.up * _laserData.Range); // nao precisa mais desse linha
            if (hit)
            {
                Collider2D collider = hit.collider;
                if (collider)
                {
                    var shieldEnemy = collider.GetComponentInParent<RiderEnemy>();
                    if (shieldEnemy) //&& shieldEnemy.IsHasShield)
                    {
                        _LaserBeam.SetPosition(1, hit.point);
                    }
                    else
                    {
                        _LaserBeam.SetPosition(1, _cannonFirePoint.transform.up * _laserData.Range);
                    }
                    IDamageable<float> damageable = collider.GetComponentInParent<EnemyHealth>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(_laserData.Damage, _laserData.LaserType);
                    }
                }
            }
            else
            {
                _LaserBeam.SetPosition(1, _cannonFirePoint.transform.up * _laserData.Range);
            }
            CurrentAmmo -= 1f / Time.time;
            CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0f, MaxAmmo);
        }
        else
        {
            _LaserBeam.enabled = false;
        }
    }

    public void Reload() => CurrentAmmo = MaxAmmo;
}
