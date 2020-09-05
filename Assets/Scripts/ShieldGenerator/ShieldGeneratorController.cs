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

    private IEnumerator _chargeTimerCoroutine;
    private WaitForSeconds _waitOneSecond;
    private WaitForSeconds _waitBarrierTimer;
    private ShieldGenerator _shieldGenerator;

    private void Awake()
    {
        EnableChargingSprite(false);
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

    private void ActivateShield()
    {
        // new behaviour with one click
        if (_chargeTimerCoroutine == null)
        {
            _chargeTimerCoroutine = ChargeTimer();
            StartCoroutine(_chargeTimerCoroutine);
        }
    }

    private IEnumerator ChargeTimer()
    {
        while (_shieldGeneratorData.ChargeTime > _shieldGenerator.ChargerTimer)
        {
            yield return _waitOneSecond;
            _energyIndicatorControl.EnergyIndicatorInstance.UpdateChargeTime(++_shieldGenerator.ChargerTimer);
        }

        if (_shieldGeneratorData.ChargeTime == _shieldGenerator.ChargerTimer)
        {
            EnableChargingSprite(true);
            _energyIndicatorControl.FillEnergyIndicatorUIAsync(OnEnergyIndicatorCharged);

            _shieldGenerator.ChargerTimer = 0f;
            yield return StartCoroutine(BarrierTimer());
        }
    }

    private void OnEnergyIndicatorCharged()
    {
        Debug.Log("Energy Indicator Full");
    }

    private IEnumerator BarrierTimer()
    {
        //---- Start the barrier timer
        _energyShield.ActivateEnergyBarrier(true);
        _energyIndicatorControl.EnergyIndicatorInstance.UpdateChargeTime(_shieldGeneratorData.ChargeTime);
        yield return _waitBarrierTimer; // wait for the barrier 
        _energyShield.ActivateEnergyBarrier(false);
        //---- Start the cooldown timer
        _shieldGenerator.CoolDownToActivated = true;

        float barrierCooldDown = _shieldGeneratorData.CoolDownTime;
        _energyIndicatorControl.EnergyIndicatorInstance.UpdateCoolDownTime(barrierCooldDown);

        EnableChargingSprite(false);

        while (barrierCooldDown >= 0.0f)
        {
            yield return _waitOneSecond;
            _energyIndicatorControl.EnergyIndicatorInstance.UpdateCoolDownTime(--barrierCooldDown);
        }
        _shieldGenerator.CoolDownToActivated = false;

        _chargeTimerCoroutine = null;

    }

    private void EnableChargingSprite(bool enable)
    {
        if (enable)
        {
            _shieldTurret.SpriteShieldTurret.sprite = _shieldGeneratorData.GetShieldSprites(1);
            _shieldControl.SpriteShieldController.sprite = _shieldGeneratorData.GetShieldSprites(3);
        }
        else
        {
            _shieldTurret.SpriteShieldTurret.sprite = _shieldGeneratorData.GetShieldSprites(0);
            _shieldControl.SpriteShieldController.sprite = _shieldGeneratorData.GetShieldSprites(2);
        }
    }

}
