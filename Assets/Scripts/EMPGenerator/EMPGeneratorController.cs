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
        EnableChargingSprite(false);
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
        // new behaviour with one click
        if (isOnActivation && _chargeTimerCoroutine == null)
        {
            _chargeTimerCoroutine = ChargetTimer();
            StartCoroutine(_chargeTimerCoroutine);
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
            EnableChargingSprite(true);
            _empGenerator.ChargerTimer = 0f;
            StopCoroutine(_chargeTimerCoroutine);
            yield return StartCoroutine(CoolDownChargeTimer());
        }
    }

    private IEnumerator CoolDownChargeTimer()
    {
        _empShockWave.PlayShockWave(_empData.ShockWaveSpeedRate); //Unleash the shock wave from here...

        _empGenerator.CoolDownToActivated = true;
        float coolDown = _empData.CoolDownTime;
        _empIndicatorControl.EnergyIndicator.UpdateCoolDownTime(coolDown);

        EnableChargingSprite(true);

        while (coolDown >= 0.0f)
        {
            yield return _waitOneSecond;
            _empIndicatorControl.EnergyIndicator.UpdateCoolDownTime(--coolDown);
        }
        EnableChargingSprite(false);
        _empGenerator.CoolDownToActivated = false;

        _chargeTimerCoroutine = null;
    }

    private void EnableChargingSprite(bool enable)
    {
        if (enable)
        {
            _empTurret.SpriteEMPTurret.sprite = _empData.GetEMPSprites(1);
            _empControl.SpriteEMPController.sprite = _empData.GetEMPSprites(3);
        }
        else
        {
            _empTurret.SpriteEMPTurret.sprite = _empData.GetEMPSprites(0);
            _empControl.SpriteEMPController.sprite = _empData.GetEMPSprites(2);
        }
    }

}