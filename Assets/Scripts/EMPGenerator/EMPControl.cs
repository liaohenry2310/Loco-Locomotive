using System;
using UnityEngine;

public class EMPControl : MonoBehaviour
{
    public event Action<bool> OnTriggerEMP;

    private InputReciever _inputReciever;

    private void Awake()
    {
        if (!TryGetComponent(out _inputReciever))
        {
            Debug.LogWarning($"[EMPGenerator] -- Failed to get the component: {_inputReciever.name}");
        }
    }

    private void Update()
    {
        OnTriggerEMP?.Invoke(_inputReciever.GetSecondaryHoldInput());
    }
}
