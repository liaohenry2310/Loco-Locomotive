using UnityEngine;

public class WeaponLaserBeam : MonoBehaviour
{
    [SerializeField] private Transform _cannonFirePoint = default;
    [SerializeField] private LaserData _laserData = default;
    [SerializeField] private LineRenderer _LaserBeam = default;

    [Header("Ammo capacity")]
    public float MaxAmmo = 100;

    public float CurrentAmmo => _laserData.Ammo;

    private void Start() => _laserData.Ammo = MaxAmmo;

    public void SetFire(bool isTrigger)
    {
        if (isTrigger && _laserData.Ammo > 0f)
        {
            if (!_LaserBeam.enabled)
            {
                _LaserBeam.enabled = true;
            }
            RaycastHit2D hit = Physics2D.Raycast(_cannonFirePoint.transform.position, _cannonFirePoint.transform.up, _laserData.Range);
            _LaserBeam.SetPosition(0, _cannonFirePoint.transform.position);
            if (hit)
            {
                _LaserBeam.SetPosition(1, hit.point);
                Collider2D collider = hit.collider;
                if (collider)
                {
                    IDamageable<float> damageable = collider.GetComponentInParent<BasicEnemy>();
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
            _laserData.Ammo -= 1f / Time.time;
            _laserData.Ammo = Mathf.Clamp(_laserData.Ammo, 0f, MaxAmmo);
        }
        else
        {
            _LaserBeam.enabled = false;
        }
    }

    public void Reload() => _laserData.Ammo = MaxAmmo;

}
