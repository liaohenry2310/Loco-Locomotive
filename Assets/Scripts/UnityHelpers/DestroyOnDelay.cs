using System.Collections;
using UnityEngine;

public class DestroyOnDelay : MonoBehaviour
{
    [SerializeField] private float _delayTime = 5.0f;

    private void Start()
    {
        BeginTimer();
    }

    public void BeginTimer()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(_delayTime);
        Destroy(gameObject);
    }
}
