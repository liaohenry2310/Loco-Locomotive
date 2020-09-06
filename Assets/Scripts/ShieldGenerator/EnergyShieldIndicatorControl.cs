using System;
using System.Collections;
using UnityEngine;

public class EnergyShieldIndicatorControl : MonoBehaviour
{
    [SerializeField] private Transform _bar = null;

    private SpriteRenderer _spriteRenderer;
    private float _chargeTimer = 0.0f;
    private float _coolDownTimer = 0.0f;

    private void Awake()
    {
        _bar.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        _spriteRenderer = _bar.GetComponentInChildren<SpriteRenderer>();
    }

    public void SetUp(float chargetTimer, float coolDownTimer)
    {
        _chargeTimer = chargetTimer;
        _coolDownTimer = coolDownTimer;
    }

    public void StartEnergyIndicatorCooldownAsync(Action callback)
    {
        StartCoroutine(StartEnergyIndicatorCooldown(callback));
    }

    private IEnumerator StartEnergyIndicatorCooldown(Action callback)
    {
        _spriteRenderer.color = Color.blue;
        float timeToCoolDown = _coolDownTimer;
        float elapsedTime = 0.0f;
        while (_bar.localScale.y > 0.0f)
        {
            timeToCoolDown -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / timeToCoolDown;
            float fillAmount = Mathf.Lerp(1.0f, 0.0f, percentage);
            _bar.localScale = new Vector3(1.0f, fillAmount, 1.0f);
            yield return null;
        }
        _bar.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        callback?.Invoke();
    }

    public void FillEnergyIndicatorUIAsync(Action onComplete)
    {
        StartCoroutine(FillEnergyIndicatorUI(onComplete));
    }

    private IEnumerator FillEnergyIndicatorUI(Action onComplete)
    {
        _spriteRenderer.color = Color.green;
        float timeToCharge = _chargeTimer;
        float elapsedTime = 0.0f;
        while (_bar.localScale.y < 1.0f)
        {
            timeToCharge -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / timeToCharge;
            float fillAmount = Mathf.Lerp(0.0f, 1.0f, percentage);
            _bar.localScale = new Vector3(1.0f, fillAmount, 1.0f);
            yield return null;
        }
        _bar.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        onComplete?.Invoke();
    }

}