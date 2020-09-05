using System;
using System.Collections;
using UnityEngine;

public class EnergyShieldIndicatorControl : MonoBehaviour
{
    [SerializeField] private ShieldGeneratorData _shieldGeneratorData = null;
    [SerializeField] private Transform _bar = null;
    [SerializeField] private float _updateSpeedSeconds = 0.5f;

    public EnergyIndicator EnergyIndicatorInstance { get; private set; }

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        EnergyIndicatorInstance = new EnergyIndicator(_shieldGeneratorData.ChargeTime, _shieldGeneratorData.CoolDownTime);
        _bar.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        _spriteRenderer = _bar.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EnergyIndicatorInstance.OnChargeTimeChanged += EnergyIndicatorChanged;
    }

    private void OnDisable()
    {
        EnergyIndicatorInstance.OnChargeTimeChanged -= EnergyIndicatorChanged;
    }

    private void EnergyIndicatorChanged(bool chargeTime)
    {
        float scaleY;
        if (chargeTime)
        {
            _spriteRenderer.color = Color.green;
            scaleY = EnergyIndicatorInstance.GetChargeTimePercent();
        }
        else
        {
            _spriteRenderer.color = Color.blue;
            scaleY = EnergyIndicatorInstance.GetCoolDonwTimePercent();
        }
        //StartCoroutine(UpdateIndicatorUI(scaleY));
        //_bar.localScale = new Vector3(1.0f, scaleY, 1.0f);
    }

    public void StartEnergyIndicatorCooldownAsync(Action callback)
    {
        StartCoroutine(StartEnergyIndicatorCooldown(callback));
    }

    private IEnumerator StartEnergyIndicatorCooldown(Action callback)
    {
        // Do you cooldown update
        yield return null;
        callback?.Invoke();
    }

    public void FillEnergyIndicatorUIAsync(Action onComplete)
    {
        StartCoroutine(FillEnergyIndicatorUI(onComplete));
    }

    private IEnumerator FillEnergyIndicatorUI(Action onComplete)
    {
        float timeToCharge = EnergyIndicatorInstance.ChargeTime;
        float elapsedTime = 0.0f;
        while (timeToCharge > 0.0f)
        {
            timeToCharge -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / timeToCharge;
            float fillAmount = Mathf.Lerp(0.0f, 1.0f, percentage);
            _bar.localScale = new Vector3(1.0f, fillAmount, 1.0f);
            yield return null;
        }
        onComplete?.Invoke();
    }

    private IEnumerator UpdateIndicatorUI(float percentage)
    {
        Debug.Log("Update Indicator UI");
        //float cachePct = EnergyIndicator._currentTime;
        float elapsed = 0.0f;
        float lerpVal = 0.0f;
        while (elapsed < _updateSpeedSeconds)
        {
            lerpVal = Mathf.Lerp(lerpVal, percentage, elapsed / Time.deltaTime);
            elapsed += Time.deltaTime;
            _bar.localScale = new Vector3(1.0f, lerpVal, 1.0f);
            yield return null;
        }
    }
}
