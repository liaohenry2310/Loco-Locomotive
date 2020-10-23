using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeUI : MonoBehaviour
{

    [Header("Fuel Gauge Properties")]
    [SerializeField]
    private float _updateSpeedSeconds = 0.5f;

    private Slider _sliderFuel;
    //private FuelController _fuelController;

    private void Awake()
    {
        _sliderFuel = GetComponentInChildren<Slider>();

       //_fuelController = FindObjectOfType<FuelController>();

        //if (_fuelController)
        //{
        //    _fuelController.OnUpdateFuelUI += UpdateFuelUI;
        //    _fuelController.OnFuelReloadUI += ReloadFuelUI;
        //}
    }

    private void UpdateFuelUI(float pct)
    {
        StartCoroutine(ChangeFuelUI(pct));
    }

    private IEnumerator ChangeFuelUI(float pct)
    {
        float cachePct = _sliderFuel.value;
        float elapsed = 0.0f;
        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            _sliderFuel.value = Mathf.Lerp(cachePct, pct, elapsed / _updateSpeedSeconds);
            yield return null;
        }
    }

    private void ReloadFuelUI(float amount)
    {
        _sliderFuel.value = amount;
        Debug.Log($"[FuelGaugeUI] {_sliderFuel.value}");
    }

}
