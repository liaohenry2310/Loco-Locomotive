using System;
using UnityEngine;

public class EnergyIndicator
{
    public event Action<bool> OnChargeTimeChanged;

    public float ChargeTime { get { return _chargeTime; } }
    private readonly float _chargeTime = 0.0f;
    private readonly float _coolDownTime = 0.0f;
    public float _currentTime = 0.0f;

    public EnergyIndicator(float chargeTime, float coolDownTime)
    {
        _chargeTime = chargeTime;
        _coolDownTime = coolDownTime;
    }

    public float GetChargeTimePercent()
    {
        return _currentTime / _chargeTime;
    }

    public float GetCoolDonwTimePercent()
    {
        return _currentTime / _coolDownTime;
    }

    public void UpdateChargeTime(float chargeTime)
    {
        _currentTime = Mathf.Clamp(chargeTime, 0.0f, _chargeTime);
        OnChargeTimeChanged?.Invoke(true);
    }

    public void UpdateCoolDownTime(float coolDown)
    {
        _currentTime = Mathf.Clamp(coolDown, 0.0f, _coolDownTime);
        OnChargeTimeChanged?.Invoke(false);
    }

}
