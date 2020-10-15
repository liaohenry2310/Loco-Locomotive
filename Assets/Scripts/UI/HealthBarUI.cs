using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Transform _bar = null;

    private Train _train;

    private void Awake()
    {
        _train = FindObjectOfType<Train>();
        _bar.localScale = new Vector3(1.0f, 1.0f, 0.0f);
    }

    private void OnEnable()
    {
        _train.OnUpdateHealthUI += UpdateHealth;
    }

    private void OnDisable()
    {
        _train.OnUpdateHealthUI -= UpdateHealth;
    }

    private void UpdateHealth(float currentHealth)
    {
        _bar.localScale = new Vector3(currentHealth, 1.0f, 0.0f);
    }
}
