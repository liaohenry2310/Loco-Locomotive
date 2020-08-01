using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeUI : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float UpdateSpeedSeconds = 0.5f;

    private Slider mSliderFuel;
    private FuelController mFuelController;

    private void Awake()
    {
        mSliderFuel = GetComponentInChildren<Slider>();
        mFuelController = FindObjectOfType<FuelController>();
        if (mFuelController)
        {
            mFuelController.OnUpdateFuelUI += UpdateFuelUI;
            mFuelController.OnFuelReloadUI += ReloadFuelUI;
        }
    }

    private void UpdateFuelUI(float pct)
    {
        StartCoroutine(ChangeFuelUI(pct));
    }

    private IEnumerator ChangeFuelUI(float pct)
    {
        float cachePct = mSliderFuel.value;
        float elapsed = 0.0f;
        while (elapsed < UpdateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            mSliderFuel.value = Mathf.Lerp(cachePct, pct, elapsed / UpdateSpeedSeconds);
            yield return null;
        }
    }

    private void ReloadFuelUI(float amount)
    {
        mSliderFuel.value = amount;
        Debug.Log($"[FuelGaugeUI] {mSliderFuel.value}");
    }


}
