using UnityEngine;

public class EnergyShieldIndicatorControl : MonoBehaviour
{
    [SerializeField] private ShieldGeneratorData _shieldGeneratorData = null;
    [SerializeField] private Transform _bar = null;
    
    public EnergyIndicator EnergyIndicator { get; private set; }

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        EnergyIndicator = new EnergyIndicator(_shieldGeneratorData.ChargeTime, _shieldGeneratorData.CoolDownTime);
        _bar.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        _spriteRenderer = _bar.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EnergyIndicator.OnChargeTimeChanged += EnergyIndicatorChanged;
    }

    private void OnDisable()
    {
        EnergyIndicator.OnChargeTimeChanged -= EnergyIndicatorChanged;
    }

    private void EnergyIndicatorChanged(bool chargeTime)
    {
        float scaleY;
        if (chargeTime)
        {
            _spriteRenderer.color = Color.green;
            scaleY = EnergyIndicator.GetChargeTimePercent();
        }
        else
        {
            _spriteRenderer.color = Color.blue;
            scaleY = EnergyIndicator.GetCoolDonwTimePercent();
        }
        _bar.localScale = new Vector3(1.0f, scaleY, 1.0f);
    }

}
