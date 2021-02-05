using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFuelBar : MonoBehaviour
{
    [SerializeField] private TrainData _trainData = null;  
    [SerializeField] private Transform _bar = null;
    private Train _train;
    private SpriteRenderer _spriteRenderer = null;

    private bool isFlickering = false;
    public float gapTime = 0.1f;
    private float tempTime;
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _train = FindObjectOfType<Train>();
        _bar.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void OnEnable()
    {
        _train.OnUpdateFuelUI += UpdateFuel;
    }

    private void OnDisable()
    {
        _train.OnUpdateFuelUI -= UpdateFuel;
    }

    private void UpdateFuel(float FuelPerc)
    {       
        _bar.localScale = new Vector3( 1.0f, FuelPerc, 1.0f);
    }
    private void Update()
    {
        tempTime += Time.deltaTime;
        if (_trainData.FuelPercentage < 0.3f)
        {
            if (tempTime >= gapTime)
            {
                Flickering();
            }
        }
    }

    public void Flickering()
    {
        Color yellow = new Color(0.988f, 0.984f, 0.239f, 1);
        Color yellow2 = new Color(0.988f, 0.984f, 0.239f, 0.25f);

        if (isFlickering)
        {
            _spriteRenderer.color = yellow2;
            isFlickering = false;
            tempTime = 0.0f;
        }
        else
        {
            _spriteRenderer.color = yellow;
            isFlickering = true;
            tempTime = 0.0f;
        }
    }
}



