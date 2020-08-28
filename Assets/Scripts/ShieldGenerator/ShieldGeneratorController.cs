using System.Collections;
using UnityEngine;

public class ShieldGeneratorController : MonoBehaviour
{
    [SerializeField] private ShieldGeneratorData _shieldGeneratorData = default;
    [SerializeField] private ShieldControl _shieldControl = default;
    [SerializeField] private ShieldTurret _shieldTurret = default;
    [SerializeField] private CircleCollider2D _shieldCollider = default;
    [SerializeField] private HealthBar _healthBar = default;

    private IEnumerator _ChargeTimerCoroutine;
    private WaitForSeconds _waitOneSecond;
    private WaitForSeconds _waitCoolDown;
    private WaitForSeconds _waitBarrierTimer;

    private ShieldGenerator _shieldGenerator;

    private void Awake()
    {
        _shieldControl.OnControllShield += ActivateShield;
        _shieldCollider.enabled = false;
    }

    private void Start()
    {
        _shieldGenerator = new ShieldGenerator(_healthBar , _shieldGeneratorData.MaxHealth);
        _shieldTurret.IMachineriesAction = _shieldGenerator;
        _waitOneSecond = new WaitForSeconds(1f);
        _waitCoolDown = new WaitForSeconds(_shieldGeneratorData.CoolDownTime);
        _waitBarrierTimer = new WaitForSeconds(_shieldGeneratorData.BarrierDuration);
    }

    private void ActivateShield(bool isOnActivation)
    {
        if (isOnActivation && !_shieldGenerator.CoolDownToActivated && !_shieldCollider.enabled)
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
                _shieldGenerator.ChargerTimer = 0f;
            }
        }
    }

    private IEnumerator ChargetTimer()
    {
        while (_shieldGeneratorData.ChargeTime > _shieldGenerator.ChargerTimer)
        {
            Debug.Log($"[ShieldGenerator] -- ChargerTime = {_shieldGenerator.ChargerTimer}");
            yield return _waitOneSecond;
            _shieldGenerator.ChargerTimer++;
        }
        if (_shieldGeneratorData.ChargeTime == _shieldGenerator.ChargerTimer)
        {
            _shieldGenerator.ChargerTimer = 0f;
            yield return StartCoroutine(BarrierTimer());
        }
    }

    private IEnumerator BarrierTimer()
    {
        _shieldCollider.enabled = true;
        Debug.Log($"[BarrierTimer] {_shieldCollider.enabled}");
        yield return _waitBarrierTimer; // wait for the barrier 
        _shieldCollider.enabled = false;
        Debug.Log($"[BarrierTimer] {_shieldCollider.enabled}");
        _shieldGenerator.CoolDownToActivated = true;
        Debug.Log($"[CoolDown] {_shieldGenerator.CoolDownToActivated}");
        yield return _waitCoolDown; // after barrier finished, start to cooldown
        _shieldGenerator.CoolDownToActivated = false;
        Debug.Log($"[CoolDown] {_shieldGenerator.CoolDownToActivated}");
        StopCoroutine(_ChargeTimerCoroutine);
    }

}
