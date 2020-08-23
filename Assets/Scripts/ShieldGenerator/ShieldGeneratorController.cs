using System.Collections;
using UnityEngine;

public class ShieldGeneratorController : MonoBehaviour
{
    [SerializeField] private ShieldGeneratorData _shieldGeneratorData;
    [SerializeField] private ShieldControl _shieldControl;
    [SerializeField] private CircleCollider2D _ShieldCollider;

    private WaitForSeconds _waitOneSecond;
    private WaitForSeconds _waitCoolDown;
    private WaitForSeconds _waitBarrierTimer;
    private IEnumerator _ChargeTimerCoroutine;

    private float _curretHealth = 0.0f;
    private float _chargerTimer = 0f;
    private bool _coolDownToActivated = false;

    private void Awake()
    {
        _shieldControl.OnControllShield += ActivateShield;
        _ShieldCollider.enabled = false;
    }

    private void Start()
    {
        _waitOneSecond = new WaitForSeconds(1f);
        _waitCoolDown = new WaitForSeconds(_shieldGeneratorData.CoolDownTime);
        _waitBarrierTimer = new WaitForSeconds(_shieldGeneratorData.BarrierDuration);
        _curretHealth = _shieldGeneratorData.MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        _curretHealth -= damage;
        _curretHealth = Mathf.Clamp(_curretHealth, 0f, _shieldGeneratorData.MaxHealth);
    }

    public bool IsAlive => _curretHealth > 0f;

    public void Repair()
    {
        _curretHealth = _shieldGeneratorData.MaxHealth;
    }

    private void ActivateShield(bool isOnActivation)
    {
        if (isOnActivation && !_coolDownToActivated)
        {
            if (_ChargeTimerCoroutine == null)
            {
                _ChargeTimerCoroutine = ChargetTimer();
                StartCoroutine(_ChargeTimerCoroutine);
            }
        }
        else
        {
            if (_ChargeTimerCoroutine != null)
            {
                StopCoroutine(_ChargeTimerCoroutine);
                _ChargeTimerCoroutine = null;
                _chargerTimer = 0f;
            }
        }
    }

    private IEnumerator ChargetTimer()
    {
        while (_shieldGeneratorData.ChargeTime > _chargerTimer)
        {
            Debug.Log($"ChargerTime = {_chargerTimer}");
            yield return _waitOneSecond;
            _chargerTimer++;
        }
        if (_shieldGeneratorData.ChargeTime == _chargerTimer)
        {
            _chargerTimer = 0f;
            yield return StartCoroutine(BarrierTimer());
        }        
    }

    private IEnumerator BarrierTimer()
    {
        _ShieldCollider.enabled = true;
        Debug.Log($"[BarrierTimer] {_ShieldCollider.enabled}");
        yield return _waitBarrierTimer; // wait for the barrier 
        _ShieldCollider.enabled = false;
        Debug.Log($"[BarrierTimer] {_ShieldCollider.enabled}");
        _coolDownToActivated = true;
        Debug.Log($"[CoolDown] {_coolDownToActivated}");
        yield return _waitCoolDown; // after start to cooldown
        _coolDownToActivated = false;
        Debug.Log($"[CoolDown] {_coolDownToActivated}");
    }

}
