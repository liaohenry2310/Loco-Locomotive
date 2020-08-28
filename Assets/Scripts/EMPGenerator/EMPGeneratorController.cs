using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPGeneratorController : MonoBehaviour
{
    [SerializeField] private EMPGeneratorData _empData = default;
    [SerializeField] private EMPControl _empControl = default;
    [SerializeField] private EMPTurret _empTurret = default;
    [SerializeField] private HealthBar _healthBar = default;

    private EMPShockWave _empShockWave = default;

    private IEnumerator _chargeTimerCoroutine;
    private WaitForSeconds _waitOneSecond;
    private WaitForSeconds _waitCoolDown;
    private EMPGenerator _empGenerator;

    private void Awake()
    {
        _empControl.OnTriggerEMP += UnleashEMP;

        _empShockWave = GetComponentInChildren<EMPShockWave>();

    }

    private void Start()
    {
        _empGenerator = new EMPGenerator(_healthBar, _empData.MaxHealth);
        _empTurret.IMachineriesAction = _empGenerator;

        _waitOneSecond = new WaitForSeconds(1f);
        _waitCoolDown = new WaitForSeconds(_empData.CoolDownTime);
    }

    private void UnleashEMP(bool isOnActivation)
    {
        if (isOnActivation && !_empGenerator.CoolDownToActivated)
        {
            if (_chargeTimerCoroutine == null)
            {
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
            }
        }
    }

    private IEnumerator ChargetTimer()
    {
        while (_empData.ChargeTime > _empGenerator.ChargerTimer)
        {
            Debug.Log($"[EMPGenerator] -- ChargerTime = {_empGenerator.ChargerTimer}");
            yield return _waitOneSecond;
            _empGenerator.ChargerTimer++;
        }
        if (_empData.ChargeTime == _empGenerator.ChargerTimer)
        {
            _empGenerator.ChargerTimer = 0f;
            _empShockWave.PlayShockWave(_empData.ShockWaveSpeedRate); //Unleash the shock wave from here...
            yield return _waitCoolDown;
        }
    }
}
