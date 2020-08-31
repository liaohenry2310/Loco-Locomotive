using System.Collections;
using UnityEngine;

public class ShieldGeneratorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShieldGeneratorData _shieldGeneratorData = null;
    [SerializeField] private ShieldControl _shieldControl = null;
    [SerializeField] private ShieldTurret _shieldTurret = null;
    [SerializeField] private EnergyShield _energyShield = null;
    [SerializeField] private HealthBar _healthBar = null;
    [SerializeField] private EnergyShieldIndicatorControl _energyIndicatorControl = null;

    private IEnumerator _ChargeTimerCoroutine;
    private WaitForSeconds _waitOneSecond;
    private WaitForSeconds _waitBarrierTimer;
    private ShieldGenerator _shieldGenerator;

    private void Awake()
    {
        _energyIndicatorControl.gameObject.SetActive(false);
    }

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
        _shieldGenerator = new ShieldGenerator(_healthBar, _shieldGeneratorData.MaxHealth);
        _shieldTurret.IReparable = _shieldGenerator;
        _shieldTurret.IDamageble = _shieldGenerator;
        _waitOneSecond = new WaitForSeconds(1f);
        _waitBarrierTimer = new WaitForSeconds(_shieldGeneratorData.BarrierDuration);
    }

    private void ActivateShield(bool isOnActivation)
    {
        if (isOnActivation && !_shieldGenerator.CoolDownToActivated && !_energyShield.IsShieldActivated)
        {
            if (_ChargeTimerCoroutine == null)
            {
                _energyIndicatorControl.gameObject.SetActive(true);
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
                if (!_energyShield.IsShieldActivated)
                {
                    _shieldGenerator.ChargerTimer = 0f;
                    _energyIndicatorControl.EnergyIndicator.UpdateChargeTime(_shieldGenerator.ChargerTimer);
                }
            }
        }
    }

    private IEnumerator ChargetTimer()
    {
        while (_shieldGeneratorData.ChargeTime > _shieldGenerator.ChargerTimer)
        {
            yield return _waitOneSecond;
            _energyIndicatorControl.EnergyIndicator.UpdateChargeTime(++_shieldGenerator.ChargerTimer);

        }
        if (_shieldGeneratorData.ChargeTime == _shieldGenerator.ChargerTimer)
        {
            _shieldGenerator.ChargerTimer = 0f;
            yield return StartCoroutine(BarrierTimer());
        }
    }

    private IEnumerator BarrierTimer()
    {
        //---- Start the barrier timer
        _energyShield.ActivateEnergyBarrier(true);
        _energyIndicatorControl.EnergyIndicator.UpdateChargeTime(_shieldGeneratorData.ChargeTime);
        yield return _waitBarrierTimer; // wait for the barrier 
        _energyShield.ActivateEnergyBarrier(false);
        //---- Start the cooldown timer
        _shieldGenerator.CoolDownToActivated = true;
        
        float barrierCooldDown = _shieldGeneratorData.CoolDownTime;
        _energyIndicatorControl.EnergyIndicator.UpdateCoolDownTime(barrierCooldDown);

        while (barrierCooldDown >= 0.0f)
        {
            yield return _waitOneSecond;
            _energyIndicatorControl.EnergyIndicator.UpdateCoolDownTime(--barrierCooldDown);
        }
        _shieldGenerator.CoolDownToActivated = false;
        _energyIndicatorControl.gameObject.SetActive(false);
    }

}
