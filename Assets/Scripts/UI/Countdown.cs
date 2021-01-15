using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Countdown : MonoBehaviour
{
    public GameObject[] elements;
    public GameObject enemySpawner;
    public float duration = 1.0f;
    public UnityEvent audioClip1;
    public UnityEvent audioClip2;

    public void StartCountdown()
    {
        StartCoroutine(RunCountdown());
    }

    private IEnumerator RunCountdown()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < elements.Length - 1; ++i)
        {
            elements[i].SetActive(true);
            StartCoroutine(AnimateElement(elements[i]));
            yield return new WaitForSeconds(duration);
            if (i < elements.Length - 2)
                elements[i].SetActive(false);
        }
        yield return new WaitForSeconds(duration * 0.25f);
        elements[elements.Length - 2].SetActive(false);
        audioClip2.Invoke();
        enemySpawner.SetActive(true);
        for (int j = 0; j < 5; ++j)
        {
            elements[elements.Length - 1].SetActive(true);
            yield return new WaitForSeconds(0.1f);
            elements[elements.Length - 1].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AnimateElement(GameObject element)
    {
        RectTransform elementTransform = element.GetComponent<RectTransform>();
        Text elementText = element.GetComponent<Text>();
        Vector2 end = elementTransform.anchoredPosition;
        Vector2 begin = new Vector2(end.x, end.y + 100.0f);

        float startTime = Time.unscaledTime;
        float t = 0.0f;

        while (t <= 1.0f)
        {
            t = (Time.unscaledTime - startTime) / (duration * 0.25f);
            elementTransform.anchoredPosition = Vector3.Lerp(begin, end, t);
            yield return null;
        }
        audioClip1.Invoke();
    }
}
