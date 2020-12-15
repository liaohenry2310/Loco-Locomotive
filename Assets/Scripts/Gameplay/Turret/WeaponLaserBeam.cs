using Interfaces;
using UnityEngine;

/// <summary>
/// ---- LEGACY CODE -----
/// Should be deleted in the FUTURE.
/// Cyro.
/// </summary>
public class WeaponLaserBeam : MonoBehaviour
{
    [SerializeField] private Transform _cannonFirePoint = null;
    [SerializeField] private LaserData _laserData = null;
    [SerializeField] private LineRenderer _LaserBeam = null;
    [SerializeField] private LayerMask _layerEnemyMask = 0;

    [Header("Ammo capacity")]
    [SerializeField] private float _maxAmmo = 30f;

    public float CurrentAmmo { get; private set; } = 0.0f;

    private void Start() => CurrentAmmo = _maxAmmo;

    public void SetFire(bool isTrigger, bool turretIsAlive = true)
    {
        // when trigger game over or turret is dead, disable the line renderer
        //if (GameManager.Instance.IsGameOver || !turretIsAlive)
        //{
        //    _LaserBeam.enabled = false;
        //    return;
        //}

        if (isTrigger && CurrentAmmo > 0f)
        {
            if (!_LaserBeam.enabled)
            {
                _LaserBeam.enabled = true;
            }

            RaycastHit2D hit = Physics2D.Raycast(_cannonFirePoint.transform.position, _cannonFirePoint.transform.up, _laserData.Range, _layerEnemyMask);
            _LaserBeam.SetPosition(0, _cannonFirePoint.transform.position);
            //_LaserBeam.SetPosition(1, _cannonFirePoint.transform.up * _laserData.Range); // nao precisa mais desse linha
            if (hit)
            {
                Collider2D collider = hit.collider;
                if (collider)
                {
                    var shieldEnemy = collider.gameObject.GetComponentInChildren<EnemyShieldHealth>();
                    if (shieldEnemy && shieldEnemy.ShieldIsActive)
                    {
                        _LaserBeam.SetPosition(1, hit.point);
                    }
                    else
                    {
                        _LaserBeam.SetPosition(1, _cannonFirePoint.transform.up * _laserData.Range);
                        IDamageableType<float> damageable = collider.GetComponent<EnemyHealth>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(_laserData.Damage * Time.deltaTime, _laserData.LaserType);
                        }
                    }
                }
            }
            else
            {
                _LaserBeam.SetPosition(1, _cannonFirePoint.transform.up * _laserData.Range);
            }
            CurrentAmmo -= 1f / Time.time;
            CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0f, _maxAmmo);
        }
        else
        {
            _LaserBeam.enabled = false;
        }
    }

    public void Reload() => CurrentAmmo = _maxAmmo;
}
