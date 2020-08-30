using System.Collections;
using UnityEngine;

public class ShieldGeneratorController : MonoBehaviour
{
    [SerializeField] private ShieldGeneratorData _shieldGeneratorData = null;
    [SerializeField] private ShieldControl _shieldControl = null;
    [SerializeField] private ShieldTurret _shieldTurret = null;
    [SerializeField] private EnergyShield _energyShield = null;
    [SerializeField] private HealthBar _healthBar = null;

    private IEnumerator _ChargeTimerCoroutine;
    private WaitForSeconds _waitOneSecond;
    private WaitForSeconds _waitCoolDown;
    private WaitForSeconds _waitBarrierTimer;

    private ShieldGenerator _shieldGenerator;
    private void OnEnable()
    {
        _shieldControl.OnControllShield += ActivateShield;
    }

    private void OnDisable()
    {
        _shieldControl.OnControllShield -= ActivateShield;
    }

    private void Start()
    {
        _shieldGenerator = new ShieldGenerator(_healthBar , _shieldGeneratorData.MaxHealth);
        _shieldTurret.IReparable = _shieldGenerator;
        _shieldTurret.IDamageble = _shieldGenerator;
        _waitOneSecond = new WaitForSeconds(1f);
        _waitCoolDown = new WaitForSeconds(_shieldGeneratorData.CoolDownTime);
        _waitBarrierTimer = new WaitForSeconds(_shieldGeneratorData.BarrierDuration);
    }

    private void ActivateShield(bool isOnActivation)
    {
        if (isOnActivation && !_shieldGenerator.CoolDownToActivated && !_energyShield.IsShieldActivated)
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
        _energyShield.ActivateEnergyBarrier(true);
        yield return _waitBarrierTimer; // wait for the barrier 
        _energyShield.ActivateEnergyBarrier(false);
        _shieldGenerator.CoolDownToActivated = true;
        yield return _waitCoolDown; // after barrier finished, start to cooldown
        _shieldGenerator.CoolDownToActivated = false;
    }

}
