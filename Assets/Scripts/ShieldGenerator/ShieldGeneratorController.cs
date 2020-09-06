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

    private WaitForSeconds _waitBarrierTimer;
    private ShieldGenerator _shieldGenerator;

    private void Awake()
    {
        EnableChargingSprite(false);
        _energyIndicatorControl.SetUp(_shieldGeneratorData.ChargeTime, _shieldGeneratorData.CoolDownTime);
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
        _waitBarrierTimer = new WaitForSeconds(_shieldGeneratorData.BarrierDuration);
    }

    private void ActivateShield()
    {
        // new behaviour with one click
        if (_shieldGenerator.IsReadyToUse)
        {
            _shieldGenerator.IsReadyToUse = false;
            _energyIndicatorControl.FillEnergyIndicatorUIAsync(OnEnergyIndicatorCharged);
        }
    }

    private void OnEnergyIndicatorCharged()
    {
        EnableChargingSprite(true);
        StartCoroutine(BarrierTimer());
    }

    private void OnEnergyIndicatorCoolDown()
    {
        _shieldGenerator.CoolDownToActivated = false;
        _shieldGenerator.IsReadyToUse = true;
        EnableChargingSprite(false);
    }

    private IEnumerator BarrierTimer()
    {
        //---- Start the barrier timer
        _energyShield.ActivateEnergyBarrier(true);
        yield return _waitBarrierTimer; // wait for the barrier 
        _energyShield.ActivateEnergyBarrier(false);

        //---- Start the cooldown timer
        _shieldGenerator.CoolDownToActivated = true;
        _energyIndicatorControl.StartEnergyIndicatorCooldownAsync(OnEnergyIndicatorCoolDown);
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
