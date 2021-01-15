using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public GameObject[] elements;
    public GameObject enemySpawner;

    public void StartCountdown()
    {
        StartCoroutine(RunCountdown());
    }

    private IEnumerator RunCountdown()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < elements.Length; ++i)
        {
            if (i == elements.Length - 1)
                enemySpawner.SetActive(true);
            elements[i].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            elements[i].SetActive(false);
        }
    }
}
