using System.Collections;
using UnityEngine;

public class FuelGaugeBarUI : MonoBehaviour
{

    [SerializeField] private Transform _bar = null;
    [SerializeField] private float _updateSpeedSeconds = 0.5f;

    private Train _train;

    private void Awake()
    {
        _train = FindObjectOfType<Train>();
        _bar.localScale = new Vector3(1.0f, 1.0f, 0.0f);
    }

    private void OnEnable()
    {
        _train.OnFuelReloadUI += FuelReload;
        _train.OnUpdateFuelUI += FuelUpdate;
    }

    private void OnDisable()
    {
        _train.OnFuelReloadUI -= FuelReload;
        _train.OnUpdateFuelUI -= FuelUpdate;
    }

    private void FuelUpdate(float percentage)
    {
        StartCoroutine(ChangeFuelUI(percentage));
    }

    private void FuelReload(float amount)
    {
        _bar.localScale = new Vector3(amount, 1.0f, 1.0f);
    }

    private IEnumerator ChangeFuelUI(float pct)
    {
        float cachePct = _bar.localScale.x;
        float elapsed = 0.0f;
        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            float cacheLerp = Mathf.Lerp(cachePct, pct, elapsed / _updateSpeedSeconds);
            _bar.localScale = new Vector3(cacheLerp, 1.0f, 1.0f);
            yield return null;
        }
    }


}
