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
    private EMPGenerator _empGenerator;

    private void Awake()
    {
        _empShockWave = GetComponentInChildren<EMPShockWave>();
        _empIndicatorControl.SetUp(_empData.ChargeTime, _empData.CoolDownTime);
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
        EnableChargingSprite(false);
    }

    private void UnleashEMP()
    {
        // new behaviour with one click
        if (_empGenerator.IsReadyToUse)
        {
            _empGenerator.IsReadyToUse = false;
            _empIndicatorControl.FillEnergyIndicatorUIAsync(OnEnergyIndicatorCharged);
        }
    }

    private void OnEnergyIndicatorCharged()
    {
        EnableChargingSprite(true);
        _empShockWave.PlayShockWave(_empData.ShockWaveSpeedRate); //Unleash the shock wave from here...
        _empGenerator.CoolDownToActivated = true;
        EnableChargingSprite(true);
        _empIndicatorControl.StartEnergyIndicatorCooldownAsync(OnEnergyIndicatorCoolDown);
    }

    private void OnEnergyIndicatorCoolDown()
    {
        _empGenerator.CoolDownToActivated = false;
        _empGenerator.IsReadyToUse = true;
        EnableChargingSprite(false);
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