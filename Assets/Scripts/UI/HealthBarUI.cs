using GamePlay;
using UnityEngine;

namespace UI
{

    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Transform _bar = null;
        private Train _train;

        private void Awake()
        {
            _train = FindObjectOfType<Train>();
            _bar.localScale = new Vector3(0.73f, 3.5f, 0.0f);
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
            _bar.localScale = new Vector3(currentHealth*0.73f, 3.5f, 0.0f);
        }
    }

}