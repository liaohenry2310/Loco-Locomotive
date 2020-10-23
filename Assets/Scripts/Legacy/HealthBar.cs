using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake() => _ = TryGetComponent(out _slider);

    public void SetHealth(float health) => _slider.value = health;

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }
}
