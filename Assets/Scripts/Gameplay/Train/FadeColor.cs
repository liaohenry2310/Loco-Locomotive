using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeColor : MonoBehaviour
{
    [SerializeField] private TrainData _trainData = null;
    private SpriteRenderer _spriteRenderer = null;                       
    private bool isFlickering = false;
    public float gapTime = 0.1f;
    private float tempTime;
    private void Awake()
    {
        if (!TryGetComponent(out _spriteRenderer))
        {
            Debug.LogWarning("Fail to load SpriteRenderer component!.");
        }
    }

    private void Update()
    {
        tempTime += Time.deltaTime;
        if (_trainData.HealthPercentage < 0.2f)
        {
            if (tempTime>= gapTime)
            {
                Flickering();
            }
        }
        else if(_trainData.HealthPercentage < 0.1f)
        {
            if (tempTime >= gapTime/2)
            {
                Flickering();
            }
        }

    }

    public void Flickering()
    {
        if (isFlickering)
        {
            _spriteRenderer.color = Color.gray;
            isFlickering = false;
            tempTime = 0.0f;
        }
        else
        {
            _spriteRenderer.color = Color.white;
            isFlickering = true;
            tempTime = 0.0f;
        }
    }
}
