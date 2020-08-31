using System.Collections;
using UnityEngine;

public class EMPGeneratorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EMPGeneratorData _empData = null;
    [SerializeField] private EMPControl _empControl = null;
    [SerializeField] private EMPTurret _empTurret = null;
    [SerializeField] private HealthBar _healthBar = null;
    [SerializeField] private EMPIndicatorControl _empIndicatorControl = null;

    private EMPShockWave _empShockWave = null;

    private IEnumerator _chargeTimerCoroutine;
    private WaitForSeconds _waitOneSecond;
    private EMPGenerator _empGenerator;

    private void Awake()
    {
        _empShockWave = GetComponentInChildren<EMPShockWave>();
        _empIndicatorControl.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _empControl.OnTriggerEMP += UnleashEMP; // Register event just after Awake
    }

    private void OnDisable()
    {
        _empControl.OnTriggerEMP -= UnleashEMP; // Unregister event right after destroy
    }

    private void Start()
    {
        _empGenerator = new EMPGenerator(_healthBar, _empData.MaxHealth);
        _empTurret.IReparable = _empGenerator;
        _empTurret.IDamageable = _empGenerator;
        _waitOneSecond = new WaitForSeconds(1f);
    }

    private void UnleashEMP(bool isOnActivation)
    {
        if (isOnActivation && !_empGenerator.CoolDownToActivated)
        {
            if (_chargeTimerCoroutine == null)
            {
                _empIndicatorControl.gameObject.SetActive(true);
                _chargeTimerCoroutine = ChargetTimer();
                StartCoroutine(_chargeTimerCoroutine);
            }
        }
        else
        {
            if (_chargeTimerCoroutine != null)
            {
                StopCoroutine(_chargeTimerCoroutine);
                _chargeTimerCoroutine = null;
                _empGenerator.ChargerTimer = 0f;
                _empIndicatorControl.EnergyIndicator.UpdateChargeTime(_empGenerator.ChargerTimer);
            }
        }
    }

    private IEnumerator ChargetTimer()
    {
        while (_empData.ChargeTime > _empGenerator.ChargerTimer)
        {
            yield return _waitOneSecond;
            _empIndicatorControl.EnergyIndicator.UpdateChargeTime(++_empGenerator.ChargerTimer);
        }
        if (_empData.ChargeTime == _empGenerator.ChargerTimer)
        {
            _empGenerator.ChargerTimer = 0f;
            yield return StartCoroutine(CoolDownChargeTimer());
        }
    }

    private IEnumerator CoolDownChargeTimer()
    {
        _empShockWave.PlayShockWave(_empData.ShockWaveSpeedRate); //Unleash the shock wave from here...

        _empGenerator.CoolDownToActivated = true;
        float coolDown = _empData.CoolDownTime;
        _empIndicatorControl.EnergyIndicator.UpdateCoolDownTime(coolDown);
        while (coolDown >= 0.0f)
        {
            yield return _waitOneSecond;
            _empIndicatorControl.EnergyIndicator.UpdateCoolDownTime(--coolDown);
        }
        _empGenerator.CoolDownToActivated = false;
        _empIndicatorControl.gameObject.SetActive(false);
    }
}
